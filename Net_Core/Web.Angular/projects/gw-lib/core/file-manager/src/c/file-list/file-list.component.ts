import { CommonModule } from '@angular/common';
import { Component, computed, inject, input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TemplateRef } from '@angular/core';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatMenuTrigger } from '@angular/material/menu';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { GWCommon } from '@growthware/common/services';
import { ISecurityInfo } from '@growthware/core/security';
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { ModalOptions, ModalService, WindowSize } from '@growthware/core/modal';
import { SecurityService } from '@growthware/core/security';
// Feature
import { FileManagerService } from '../../file-manager.service';
import { IFileInfoLight } from '../../interfaces/file-info-light.model';

@Component({
	selector: 'gw-core-file-list',
	standalone: true,
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		// Angular Material
		MatButtonModule,
		MatIconModule,
		MatMenuModule,
		MatMenuTrigger,
		MatTabsModule,
	],
	templateUrl: './file-list.component.html',
	styleUrls: ['./file-list.component.scss']
})
export class FileListComponent implements OnDestroy, OnInit {
	private _FileManagerSvc = inject(FileManagerService);
	private _FormBuilder = inject(FormBuilder);
	private _LoggingSvc = inject(LoggingService);
	private _ModalSvc = inject(ModalService);
	private _Router = inject(Router);
	private _SecuritySvc = inject(SecurityService);

	private _Action: string = '';
	private _ModalId_Delete = 'FileListComponent.onMenuDeleteClick';
	private _ModalId_Properties = 'FileListComponent.onMenuPropertiesClick';
	private _ModalId_Rename: string = 'FileListComponent.onRenameClick';

	readonly data$ = computed<Array<IFileInfoLight>>(() => this._FileManagerSvc.filesChanged$());

	id = input.required<string>();
	frmRenameFile!: FormGroup;
	menuTopLeftPosition = { x: '0', y: '0' }; // we create an object that contains coordinates 
	selectedFile!: IFileInfoLight;
	showDelete: boolean = false;
	showDownload: boolean = false;
	showRename: boolean = false;

	numberOfColumns = input<number>(4);

	// reference to the MatMenuTrigger in the DOM 
	@ViewChild(MatMenuTrigger, { static: true }) private _MatMenuTrigger!: MatMenuTrigger;
	@ViewChild('deleteFile', { read: TemplateRef }) private _DeleteFile!: TemplateRef<unknown>;
	@ViewChild('fileProperties', { read: TemplateRef }) private _FileProperties!: TemplateRef<unknown>;
	@ViewChild('renameFile', { read: TemplateRef }) private _RenameFile!: TemplateRef<unknown>;

	ngOnDestroy(): void {
		this._Action = '';
	}

	ngOnInit(): void {
		this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
		this.id.apply(this._Action + '_Files');
		this._SecuritySvc.getSecurityInfo(this._Action).then((response: ISecurityInfo) => {
			this.showDownload = response.mayView;
			this.showDelete = response.mayDelete;
			this.showRename = response.mayEdit;
		}).catch((error) => {
			this._LoggingSvc.errorHandler(error, 'FileListComponent', 'ngOnInit');
		});

	}

	get getControls() {
		return this.frmRenameFile.controls;
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
	 * Get the style for template columns.
	 *
	 * @return {object} the style for template columns
	 */
	getTemplateColumnsStyle(): object {
		const mRetVal = { 'grid-template-columns': 'repeat(' + Number(this.numberOfColumns()) + ', 1fr)' };
		// console.log('getTemplateColumnsStyle.mRetVal', mRetVal);
		return mRetVal;
	}

	/**
	 * Handle left click event on the file
	 *
	 * @param {IFileInfoLight} item - the file information
	 */
	onLeftClick(item: IFileInfoLight) {
		if (!this.selectedFile) {
			this.selectedFile = item;
		} else {
			if (this.selectedFile.name !== item.name) {
				this.selectedFile = item;
			} else {
				this.selectedFile.name = '';
			}
		}
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
		const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Rename, 'Rename File', this._RenameFile, new WindowSize(125, 400));
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
		this._MatMenuTrigger.menuData = { item: item };

		// we open the menu
		this._MatMenuTrigger.openMenu();
	}
}

