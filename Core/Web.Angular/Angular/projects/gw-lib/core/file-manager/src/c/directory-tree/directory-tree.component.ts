import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ViewChild } from '@angular/core';
import { NestedTreeControl } from '@angular/cdk/tree';
import { TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
// Library
import { INameDataPair } from '@growthware/common/interfaces';
import { DataService } from '@growthware/common/services';
import { GWCommon } from '@growthware/common/services';
import { LogDestination, ILogOptions, LogOptions } from '@growthware/core/logging';
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { ModalOptions, ModalService, WindowSize } from '@growthware/core/modal';
import { ISecurityInfo, SecurityInfo } from '@growthware/core/security';
import { SecurityService } from '@growthware/core/security';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuTrigger } from '@angular/material/menu';
import { MatMenuModule } from '@angular/material/menu';
import { MatTreeModule } from '@angular/material/tree';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { ScrollingModule } from '@angular/cdk/scrolling';
// Library
import { IDirectoryTree } from '@growthware/common/interfaces';
// Feature
import { FileManagerService } from '../../file-manager.service';
import { RenameDirectoryComponent } from '../rename-directory/rename-directory.component';


@Component({
	selector: 'gw-core-directory-tree',
	standalone: true,
	imports: [
		CommonModule,

		MatButtonModule,
		MatIconModule,
		MatMenuModule,
		MatTreeModule,
		ScrollingModule,
	],
	templateUrl: './directory-tree.component.html',
	styleUrls: ['./directory-tree.component.scss']
})
export class DirectoryTreeComponent implements OnInit {

	private _Action: string = '';
	private _ModalId_Directory_Delete = 'DirectoryTreeComponent.onMenuDeleteClick';
	private _ModalId_Directory_Properties = 'DirectoryTreeComponent.onMenuPropertiesClick';
	private _SecurityInfo: ISecurityInfo = new SecurityInfo();
	private _Subscriptions: Subscription = new Subscription();

	@Input() doGetFiles: boolean = true;

	activeNode?: IDirectoryTree;
	configurationName: string = '';
	menuTopLeftPosition = { x: '0', y: '0' }; // we create an object that contains coordinates
	selectedPath: string = '';

	dataSource = new MatTreeNestedDataSource<IDirectoryTree>();
	treeControl = new NestedTreeControl<IDirectoryTree>(node => node.children);
	showDelete: boolean = false;
	showRename: boolean = false;

	// reference to the MatMenuTrigger in the DOM 
	@ViewChild(MatMenuTrigger, { static: true }) private _MatMenuTrigger!: MatMenuTrigger;
	@ViewChild('deleteDirectory', { read: TemplateRef }) private _DeleteDirectory!: TemplateRef<unknown>;
	@ViewChild('directoryProperties', { read: TemplateRef }) private _DirectoryProperties!: TemplateRef<unknown>;

	constructor(
		private _DataSvc: DataService,
		private _FileManagerSvc: FileManagerService,
		private _GWCommon: GWCommon,
		private _LoggingSvc: LoggingService,
		private _ModalSvc: ModalService,
		private _Router: Router,
		private _SecuritySvc: SecurityService
	) {
		// do nothing
	}

	ngOnInit(): void {
		this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
		this.configurationName = this._Action + '_Directories';
		if (!this._GWCommon.isNullOrEmpty(this.configurationName)) {
			this._SecuritySvc.getSecurityInfo(this._Action).then((response: ISecurityInfo) => {
				this._SecurityInfo = response;
			}).catch((error) => {
				this._LoggingSvc.errorHandler(error, 'FileListComponent', 'ngOnInit');
			});
			// logic to start getting data
			this._Subscriptions.add(this._DataSvc.dataChanged$.subscribe((data: INameDataPair) => {
				if (data.name.toLowerCase() === this.configurationName.toLowerCase()) {
					// console.log('data.value', data.value);
					this.dataSource.data = data.value;
					if (this.doGetFiles) {
						const mAction = this.configurationName.replace('_Directories', '');
						const mForControlName = mAction + '_Files';
						this.activeNode = data.value[0];
						this.selectedPath = data.value[0].relitivePath;
						this._FileManagerSvc.getFiles(mAction, mForControlName, data.value[0].relitivePath);
					}
				}
			}));
			this._Subscriptions.add(this._FileManagerSvc.selectedDirectoryChanged.subscribe((data: IDirectoryTree) => {
				this.selectedPath = data.relitivePath;
				this.activeNode = data;
				this.expand(this.dataSource.data, data.relitivePath);
			}));
		} else {
			const mLogDestinations: Array<LogDestination> = [];
			mLogDestinations.push(LogDestination.Console);
			mLogDestinations.push(LogDestination.Toast);
			const mLogOptions: ILogOptions = new LogOptions(
				'DirectoryTreeComponent.ngOnInit: configurationName is blank',
				LogLevel.Error,
				mLogDestinations,
				'DirectoryTreeComponent',
				'DirectoryTreeComponent',
				'ngOnInit',
				'system',
				'DirectoryTreeComponent'
			);
			this._LoggingSvc.log(mLogOptions);
		}
	}

	/**
	 * Expands all the parent nodes given the hierarchical data and the value of the unique node property
	 *
	 * @param {IDirectoryTree[]} data
	 * @param {string} relitivePath
	 * @return {*}  {*}
	 * @memberof DirectoryTreeComponent
	 */
	expand(data: IDirectoryTree[], relitivePath: string): void {
		data.forEach(node => {
			if (node.children && node.children.find(c => c.relitivePath === relitivePath)) {
				this.treeControl.expand(node);
				this.expand(data, node.relitivePath);
			}
			else if (node.children && node.children.find(c => c.children)) {
				this.expand(node.children, relitivePath);
			}
		});
	}

	hasChild = (_: number, node: IDirectoryTree) => !!node.children && node.children.length > 0;

	/**
	 * @description Handles the delete menu click
	 *
	 * @param {IDirectoryTree} item The tree node
	 * @memberof DirectoryTreeComponent
	 */
	onMenuDeleteClick(item: IDirectoryTree) {
		// console.log('item', item);
		const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Directory_Delete, 'Delete Directory', this._DeleteDirectory, new WindowSize(84, 300));
		mModalOptions.buttons.okButton.visible = true;
		mModalOptions.buttons.okButton.text = 'Yes';
		mModalOptions.buttons.okButton.callbackMethod = () => {
			this._FileManagerSvc.deleteDirectory(this._Action, this.selectedPath).then(() => {
				const mPreviousRelitavePath = this.previousRelitavePath(item);
				this._FileManagerSvc.getDirectories(this._Action, mPreviousRelitavePath, this.configurationName).then(() => {
					const mPreviousDirectoryNode: IDirectoryTree = <IDirectoryTree>this._GWCommon.hierarchySearch(this.dataSource.data, mPreviousRelitavePath, 'relitivePath', 'children');
					this.selectDirectory(mPreviousDirectoryNode);
					this._ModalSvc.close(this._ModalId_Directory_Delete);
				}).catch((error) => {
					this._LoggingSvc.errorHandler(error, 'DirectoryTreeComponent', 'onMenuDeleteClick');
				});
			}).catch((error) => {
				this._LoggingSvc.errorHandler(error, 'DirectoryTreeComponent', 'onMenuDeleteClick');
				this._LoggingSvc.toast('Was not able to delete the directory', 'Delete directory error', LogLevel.Error);
			});
		};
		this._ModalSvc.open(mModalOptions);
	}

	/**
	 * @description Handles the properties menu click
	 *
	 * @param {IDirectoryTree} item The tree node
	 * @memberof DirectoryTreeComponent
	 */
	onMenuPropertiesClick(item: IDirectoryTree) {
		// console.log('item', item);
		// this._FileManagerSvc.CurrentDirectoryTree = item;
		this.activeNode = item;
		const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Directory_Properties, 'Properties', this._DirectoryProperties, new WindowSize(80, 600));
		mModalOptions.buttons.okButton.visible = true;
		mModalOptions.buttons.okButton.callbackMethod = () => {
			this._ModalSvc.close(this._ModalId_Directory_Properties);
		};
		this._ModalSvc.open(mModalOptions);
	}

	/**
	 * @description Handles the rename menu click
	 *
	 * @param {IDirectoryTree} item The tree node
	 * @memberof DirectoryTreeComponent
	 */
	onMenuRenameClick(item: IDirectoryTree) {
		console.log('item', item);
		const mModalOptions: ModalOptions = new ModalOptions(this._FileManagerSvc.ModalId_Rename_Directory, 'Rename Directory', RenameDirectoryComponent, new WindowSize(84, 300));
		this._ModalSvc.open(mModalOptions);
	}

	/**
	 * Method called when the user click with the right button
	 * @param event MouseEvent, it contains the coordinates
	 * @param item Our data contained in the row of the table
	 */
	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	onRightClick(event: MouseEvent, item: any) {
		// console.log('item.content', item.content);
		this.onSelectDirectory(item.content);
		// preventDefault avoids to show the visualization of the right-click menu of the browser
		event.preventDefault();

		// we record the mouse position in our object
		this.menuTopLeftPosition.x = event.clientX.toString();
		this.menuTopLeftPosition.y = event.clientY.toString();

		// we open the menu
		// we pass to the menu the information about our object
		this._MatMenuTrigger.menuData = { item: item };

		// we open the menu
		this._MatMenuTrigger.openMenu();
	}

	/**
	 * @description Handles the when the tree node has been clicked
	 *
	 * @param {IDirectoryTree} node
	 * @memberof DirectoryTreeComponent
	 */
	onSelectDirectory(node: IDirectoryTree): void {
		this.selectDirectory(node);
	}

	/**
	 * @description Return the previous directory node's relitive path
	 *
	 * @private
	 * @param {IDirectoryTree} directoryTree
	 * @return {*}  {string}
	 * @memberof DirectoryTreeComponent
	 */
	private previousRelitavePath(directoryTree: IDirectoryTree): string {
		const mRelitivePathParts = directoryTree.relitivePath.split('\\');
		let mRetVal: string = '';
		for (let index = 1; index < mRelitivePathParts.length - 1; index++) {
			mRetVal += '\\' + mRelitivePathParts[index];
		}
		return mRetVal;
	}

	/**
	 * @description Performs the logic for a select directory event
	 *
	 * @private
	 * @param {IDirectoryTree} directoryTree
	 * @memberof DirectoryTreeComponent
	 */
	private selectDirectory(directoryTree: IDirectoryTree): void {
		this.showDelete = false;
		this.showRename = false;
		if (directoryTree.relitivePath.length !== 0) {
			this.showDelete = this._SecurityInfo.mayDelete;
			this.showRename = this._SecurityInfo.mayEdit;
		}
		this._FileManagerSvc.setSelectedDirectory(directoryTree);
		if (this.doGetFiles) {
			const mAction = this.configurationName.replace('_Directories', '');
			const mForControlName = mAction + '_Files';
			this._FileManagerSvc.getFiles(mAction, mForControlName, directoryTree.relitivePath);
		}
	}
}
