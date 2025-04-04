import { CommonModule } from '@angular/common';
import { Component, effect, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NestedTreeControl } from '@angular/cdk/tree';
import { Router } from '@angular/router';
// Library
import { GWCommon } from '@growthware/common/services';
import { IDirectoryTree } from '@growthware/common/interfaces';
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { ModalOptions, ModalService, WindowSize } from '@growthware/core/modal';
import { ISecurityInfo, SecurityInfo, SecurityService } from '@growthware/core/security';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule, MatMenuTrigger } from '@angular/material/menu';
import { MatTreeModule, MatTreeNestedDataSource } from '@angular/material/tree';
import { ScrollingModule } from '@angular/cdk/scrolling';
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
export class DirectoryTreeComponent implements OnDestroy, OnInit {

	private _Action: string = '';
	private _ModalId_Directory_Delete = 'DirectoryTreeComponent.onMenuDeleteClick';
	private _ModalId_Directory_Properties = 'DirectoryTreeComponent.onMenuPropertiesClick';
	private _SecurityInfo: ISecurityInfo = new SecurityInfo();

	activeNode?: IDirectoryTree;
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
		private _FileManagerSvc: FileManagerService,
		private _GWCommon: GWCommon,
		private _LoggingSvc: LoggingService,
		private _ModalSvc: ModalService,
		private _Router: Router,
		private _SecuritySvc: SecurityService
	) {
		// logic to consume data (directories and files)
		effect(() => {
			const mDirectories = this._FileManagerSvc.directoriesChanged$();
			this.dataSource.data = mDirectories;
			const mSelectedDirectory = this._GWCommon.hierarchySearch(mDirectories, this._FileManagerSvc.selectedPath, 'relitivePath') as IDirectoryTree;
			if (mSelectedDirectory) {
				this.expand(this.dataSource.data, mSelectedDirectory.relitivePath);
				this.activeNode = mSelectedDirectory;
			}
		});
		effect(() => {
			const mSelectedDirectory = this._FileManagerSvc.selectedDirectoryChanged$();
			if (this.selectedPath !== mSelectedDirectory.relitivePath || this._FileManagerSvc.needToExpand) {
				this.selectedPath = mSelectedDirectory.relitivePath;
				if (this._GWCommon.isNullOrEmpty(mSelectedDirectory.relitivePath)) {
					this.selectedPath = '\\';
				}
				this.activeNode = mSelectedDirectory;
				this.expand(this.dataSource.data, mSelectedDirectory.relitivePath);
				this._FileManagerSvc.getFiles(this._Action, this.selectedPath);
				if (this._FileManagerSvc.needToExpand) {
					this._FileManagerSvc.needToExpand = false;
				}
			}
		});
	}

	ngOnDestroy(): void {
		this._Action = '';
	}

	ngOnInit(): void {
		this._FileManagerSvc.setSelectedDirectory('\\');
		this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
		this._SecuritySvc.getSecurityInfo(this._Action).then((response: ISecurityInfo) => {
			this._SecurityInfo = response;
		}).catch((error) => {
			this._LoggingSvc.errorHandler(error, 'FileListComponent', 'ngOnInit');
		});
		this._FileManagerSvc.getFiles(this._Action, this.selectedPath);
	}

	/**
	 * Expands all the parent nodes given the hierarchical data and the value of the unique node property
	 *
	 * @param {IDirectoryTree[]} data - description of parameter
	 * @param {string} relitivePath - description of parameter
	 * @return {void} description of return value
	 * @memberof DirectoryTreeComponent
	 */
	expand(data: IDirectoryTree[], relitivePath: string): void {
		this.treeControl.expand(data[0]);
		for (let i: number = 0; i < data.length; i++) {
			const node = data[i];
			if (node.children && node.children.find(c => c.relitivePath === relitivePath)) {
				this.treeControl.expand(node);
				break;
			}
			else if (node.children && node.children.find(c => c.children)) {
				this.expand(node.children, relitivePath);
			}
		}
	}

	hasChild = (_: number, node: IDirectoryTree) => !!node.children && node.children.length > 0;

	/**
	 * @description Handles the delete menu click
	 *
	 * @param {IDirectoryTree} item The tree node
	 * @memberof DirectoryTreeComponent
	 */
	onMenuDeleteClick(item: IDirectoryTree) {
		console.log('item', item);
		this.selectedPath = item.relitivePath;
		const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Directory_Delete, 'Delete Directory', this._DeleteDirectory, new WindowSize(84, 300));
		mModalOptions.buttons.okButton.visible = true;
		mModalOptions.buttons.okButton.text = 'Yes';
		mModalOptions.buttons.okButton.callbackMethod = () => {
			this._FileManagerSvc.deleteDirectory(this._Action, this.selectedPath).then(() => {
				// TODO: get files for the selected directory
				this._ModalSvc.close(this._ModalId_Directory_Delete);
			}).catch((error) => {
				this._LoggingSvc.errorHandler(error, 'DirectoryTreeComponent', 'onMenuDeleteClick');
				this._LoggingSvc.toast('Was not able to delete the directory', 'Delete directory error', LogLevel.Error);
				this._ModalSvc.close(this._ModalId_Directory_Delete);
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
		const mModalOptions: ModalOptions = new ModalOptions(this._FileManagerSvc.MODAL_ID_RENAME_DIRECTORY, 'Rename Directory', RenameDirectoryComponent, new WindowSize(84, 300));
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
		this._FileManagerSvc.setSelectedDirectory(directoryTree.relitivePath);
	}
}
