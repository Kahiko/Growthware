import { AfterViewInit, Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
// Angular Material
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatMenuModule, MatMenuTrigger } from '@angular/material/menu';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
// Library
import { GWCommon } from '@growthware/common/services';
import { LogLevel, LoggingService } from '@growthware/core/logging';
import { ModalOptions, ModalService, WindowSize } from '@growthware/core/modal';
import { ISecurityInfo, SecurityService } from '@growthware/core/security';
// Feature
import { IFileInfoLight } from '../../interfaces/file-info-light.model';
import { FileManagerService } from '../../file-manager.service';

@Component({
	selector: 'gw-core-table-file-list',
	standalone: true,
	imports: [
		FormsModule,
		ReactiveFormsModule,
		
		MatFormFieldModule,
		MatInputModule,
		MatMenuModule,
		MatMenuTrigger,
		MatSortModule,
		MatTableModule,
		MatPaginatorModule
	],
	templateUrl: './table-file-list.component.html',
	styleUrl: './table-file-list.component.scss'
})
export class TableFileListComponent implements AfterViewInit, OnDestroy, OnInit {
	private _Action = '';
	private _ModalId_Delete = 'FileListComponent.onMenuDeleteClick';
	private _ModalId_Properties = 'FileListComponent.onMenuPropertiesClick';
	private _ModalId_Rename: string = 'FileListComponent.onRenameClick';

	dataSource = new MatTableDataSource<IFileInfoLight>([]);
	// displayedColumns pulled from IFileInfoLight Note: order here effects the order in the table
	displayedColumns: string[] = [
		'fullName',
		'createdShort',
		'modifiedShort',
		'size'
	];
	id: string = '';
	frmRenameFile!: FormGroup;
	menuTopLeftPosition = { x: '0', y: '0' }; // we create an object that contains coordinates 
	selectedFile!: IFileInfoLight;
	showDelete: boolean = false;
	showDownload: boolean = false;
	showRename: boolean = false;

	@ViewChild(MatPaginator) paginator!: MatPaginator;
	@ViewChild(MatSort) sort!: MatSort;
	// reference to the MatMenuTrigger in the DOM 
	@ViewChild( MatMenuTrigger, {static: true}) private _MatMenuTrigger!: MatMenuTrigger;
	@ViewChild('deleteFile', { read: TemplateRef }) private _DeleteFile!:TemplateRef<unknown>;
	@ViewChild('fileProperties', { read: TemplateRef }) private _FileProperties!:TemplateRef<unknown>;
	@ViewChild('renameFile', { read: TemplateRef }) private _RenameFile!:TemplateRef<unknown>;
	
	constructor(
		private _FileManagerSvc: FileManagerService,
		private _FormBuilder: FormBuilder,
		private _GWCommon: GWCommon,
		private _LoggingSvc: LoggingService,
		private _ModalSvc: ModalService,
		private _Router: Router,
		private _SecuritySvc: SecurityService
	) { }

	ngOnDestroy(): void {
		this._Action = '';
	}

	ngOnInit(): void {
		this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
		this._SecuritySvc.getSecurityInfo(this._Action).then((response: ISecurityInfo) => {
			this.showDownload = response.mayView;
			this.showDelete = response.mayDelete;
			this.showRename = response.mayEdit;
		}).catch((error) => {
			this._LoggingSvc.errorHandler(error, 'FileListComponent', 'ngOnInit');
		});
		if (this._GWCommon.isNullOrUndefined(this.id)) {
			this._LoggingSvc.toast('The id can not be blank!', 'File List Component', LogLevel.Error);
		} else {
			this.dataSource.data = this._FileManagerSvc.filesChanged$();
		}
	}

	/**
	 * Get error message for a specific field.
	 *
	 * @param {string} fieldName - the name of the field
	 * @return {string | undefined} the error message, or undefined if no error
	 */
	getErrorMessage(fieldName: string): string | undefined {
		switch (fieldName) {
		case 'newName':
			if (this.getControls['newPassword'].hasError('required')) {
				return 'Required';
			}
			break;
		default:
			break;
		}
		return undefined;
	}
	/**
	 * Returns the controls of the 'frmRenameFile' form.
	 *
	 * @return {any} The controls of the 'frmRenameFile' form.
	 */
	get getControls() {
		return this.frmRenameFile.controls;
	}

	ngAfterViewInit() {
		this.dataSource.paginator = this.paginator;
		this.dataSource.sort = this.sort;
		this.sort.sortChange.subscribe(() => {
			if (this.sort.active === 'size') {
				this.dataSource.data = this.dataSource.data.sort(this.compareSize.bind(this));
			}
		});
	}

	applyFilter(event: Event) {
		const filterValue = (event.target as HTMLInputElement).value;
		this.dataSource.filter = filterValue.trim().toLowerCase();

		if (this.dataSource.paginator) {
			this.dataSource.paginator.firstPage();
		}
	}

	// Custom sorting function for size
	compareSize(a: IFileInfoLight, b: IFileInfoLight): number {
		const sizeA = this._FileManagerSvc.convertSizeToBytes(a.size);
		const sizeB = this._FileManagerSvc.convertSizeToBytes(b.size);
		return sizeA - sizeB;
	}

	/**
	 * Handle "Delete" menu click event
	 *
	 * @param {IFileInfoLight} item - the file information
	 */
	onMenuDeleteClick(item: IFileInfoLight) {
		// console.log('item', item);
		this.selectedFile = item;
		const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Delete, 'Delete File', this._DeleteFile, new WindowSize(84, 300));
		mModalOptions.buttons.okButton.visible = true;
		mModalOptions.buttons.okButton.text = 'Yes';
		mModalOptions.buttons.okButton.callbackMethod = () => {
			this._FileManagerSvc.deleteFile(this._Action, this.selectedFile.name).then(() => {
				this._FileManagerSvc.getFiles(this._Action, this._FileManagerSvc.selectedPath);
				this._LoggingSvc.toast('File was deleted', 'Delete file', LogLevel.Success);
			}).catch((error) => {
				this._LoggingSvc.errorHandler(error, 'FileListComponent', 'onMenuDeleteClick');
				this._LoggingSvc.toast('Was not able to delete the file', 'Delete file error', LogLevel.Error);
			});
			this._ModalSvc.close(this._ModalId_Delete);
		};
		this._ModalSvc.open(mModalOptions);
	}

	/**
	 * Handle "Download" menu click event
	 *
	 * @param {IFileInfoLight} item - the file information
	 */
	onMenuDownloadClick(item: IFileInfoLight) {
		// console.log('item', item);
		this.selectedFile = item;
		this._FileManagerSvc.getFile(this._Action, this._FileManagerSvc.selectedPath, item.name);
	}

	/**
	 * Handle "Rename" menu click event
	 *
	 * @param {IFileInfoLight} item - the file information
	 */
	onMenuRenameClick(item: IFileInfoLight) {
		// console.log('item', item);
		this.selectedFile = item;
		this.populateRenameFileForm(this.selectedFile.name);
		const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Rename, 'Rename File', this._RenameFile, new WindowSize(84, 300));
		this._ModalSvc.open(mModalOptions);
	}

	/**
	 * Handle "Properties" menu click event
	 *
	 * @param {IFileInfoLight} item - the file information
	 * @return {void} 
	 */
	onMenuPropertiesClick(item: IFileInfoLight) {
		this.selectedFile = item;
		const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Properties, 'Properties', this._FileProperties, new WindowSize(80, 600));
		mModalOptions.buttons.okButton.visible = true;
		mModalOptions.buttons.okButton.callbackMethod = () => {
			this._ModalSvc.close(this._ModalId_Properties);
		};
		this._ModalSvc.open(mModalOptions);
	}
	/**
	 * Method called when the user click with the right button
	 * @param event MouseEvent, it contains the coordinates
	 * @param item Our data contained in the row of the table
	 */
	onRightClick(event: MouseEvent, item: IFileInfoLight) {
		this.selectedFile = item;
		// preventDefault avoids to show the visualization of the right-click menu of the browser
		event.preventDefault();

		// we record the mouse position in our object
		this.menuTopLeftPosition.x = event.clientX.toString();
		this.menuTopLeftPosition.y = event.clientY.toString();

		// we open the menu
		// we pass to the menu the information about our object
		this._MatMenuTrigger.menuData = {item: item};

		// we open the menu
		this._MatMenuTrigger.openMenu();
	}
	/**
	 * Performs the rename by calling _FileManagerSvc.renameFile.
	 *
	 * @param {FormGroup} form - description of parameter
	 * @return {void} description of return value
	 */
	onRenameSubmit(form: FormGroup): void {
		this._FileManagerSvc.renameFile(this._Action, this.selectedFile.name, form.value['newFileName']).then(() => {
			form.reset();
			this._ModalSvc.close(this._ModalId_Rename);
			this._LoggingSvc.toast('File was renamed', 'Rename file', LogLevel.Success);
		}).catch((error) => {
			this._LoggingSvc.errorHandler(error, 'FileListComponent', 'onRenameSubmit');
			this._LoggingSvc.toast('File was NOT renamed', 'Rename file', LogLevel.Success);
		});
	}

	private populateRenameFileForm(fileName: string): void {
		this.frmRenameFile = this._FormBuilder.group({
			newFileName: [fileName, [Validators.required]],
		});
	}
}
