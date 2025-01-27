import { CommonModule } from '@angular/common';
import { Component, computed, effect, inject, input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TemplateRef } from '@angular/core';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatMenuTrigger } from '@angular/material/menu';
import { MatSelectModule } from '@angular/material/select';
// Library
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
		MatFormFieldModule,
		MatIconModule,
		MatInputModule,
		MatLabel,
		MatMenuModule,
		MatMenuTrigger,
		MatSelectModule,
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
	files: IFileInfoLight[] = []; // Regular property to hold the files
	filterTerm: string = ""; // Property to hold the filter term
	frmRenameFile!: FormGroup;
	menuTopLeftPosition = { x: '0', y: '0' }; // we create an object that contains coordinates
	selectUnselectText: string = 'Select';
	selectedFile!: IFileInfoLight;
	selectedSortOption: string = ""; // Default to empty or the first option
	showDelete: boolean = false;
	showDownload: boolean = false;
	showRename: boolean = false;

	numberOfColumns = input<number>(4);

	// reference to the MatMenuTrigger in the DOM 
	@ViewChild(MatMenuTrigger, { static: true }) private _MatMenuTrigger!: MatMenuTrigger;
	@ViewChild('deleteFile', { read: TemplateRef }) private _DeleteFile!: TemplateRef<unknown>;
	@ViewChild('deleteSelected', { read: TemplateRef }) private _DeleteSelected!: TemplateRef<unknown>;
	@ViewChild('fileProperties', { read: TemplateRef }) private _FileProperties!: TemplateRef<unknown>;
	@ViewChild('renameFile', { read: TemplateRef }) private _RenameFile!: TemplateRef<unknown>;

	constructor() {
		// make sure that the local files property is updated should the files change
		// in the service
		effect(() => {
			this.files = this.data$();
			this.selectedSortOption = ""; // Reset to default sorting option
		});
	}

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

	/**
	 * Checks if all the files are selected, and if so, changes the text of the
	 * select/unselect button to 'Unselect'. If not, changes the text to 'Select'.
	 * @returns {boolean} true if all files are selected, false otherwise
	 */
	get allSelected(): boolean {
		const isChecked = this.data$().every(file => file.selected);
		this.selectUnselectText = isChecked ? 'Unselect' : 'Select';
		return isChecked;
	}

	/**
	 * Determines if any of the files are selected.
	 *
	 * @returns {boolean} true if any of the files are selected, false otherwise
	 */
	get anySelected(): boolean {
		return this.data$().some(file => file.selected);
	}

	get getControls() {
		return this.frmRenameFile.controls;
	}

	/**
	 * Handles the "Delete Selected" button click event.
	 *
	 * @returns {void}
	 */
	onDeleteSelected() {
		const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Delete, 'Delete Selected Files', this._DeleteSelected, new WindowSize(84, 300));
		mModalOptions.buttons.okButton.visible = true;
		mModalOptions.buttons.okButton.text = 'Yes';
		mModalOptions.buttons.okButton.callbackMethod = () => {
			this._FileManagerSvc.deleteFiles(this._Action).then(() => {
				this._FileManagerSvc.getFiles(this._Action, this._FileManagerSvc.selectedPath);
				this._LoggingSvc.toast('Files were deleted', 'Delete files', LogLevel.Success);
			}).catch((error) => {
				this._LoggingSvc.errorHandler(error, 'FileListComponent', 'onDeleteSelected');
				this._LoggingSvc.toast('Was not able to delete the files', 'Delete files error', LogLevel.Error);
			});			
			this._ModalSvc.close(this._ModalId_Delete);
		};
		this._ModalSvc.open(mModalOptions);
	}

	/**
	 * Filter the files based on the search term changed.
	 *
	 * @param {Event} event The event from the input element.
	 *
	 * Updates the displayed files based on the filter.
	 */
	onFilterChange(event: Event) {
		const target = event.target as HTMLInputElement;
		const searchTerm = target.value ?? '';
		const mFiles = [...this.data$()].filter(file => file.shortFileName.toLowerCase().includes(searchTerm.toLowerCase()));
		this.files = mFiles; // Update the displayed files based on the filter
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

	/**
	 * Handle "Sort" option click event
	 * @param {string} sortType - the type of sorting, one of
	 * - 'name-asc': sort by name in ascending order
	 * - 'name-desc': sort by name in descending order
	 * - 'date-asc': sort by date in ascending order
	 * - 'date-desc': sort by date in descending order
	 * - 'size-asc': sort by size in ascending order
	 * - 'size-desc': sort by size in descending order
	 */
	onSortChange(sortType: string) {
		if (!sortType) return;
		const mSortArray = [...this.files];
	  
		switch (sortType) {
		  case 'name-asc':
			mSortArray.sort((a, b) => a.shortFileName.localeCompare(b.shortFileName));
			break;
		  case 'name-desc':
			mSortArray.sort((a, b) => b.shortFileName.localeCompare(a.shortFileName));
			break;
		  case 'date-asc':
			mSortArray.sort((a, b) => new Date(a.created).getTime() - new Date(b.created).getTime());
			break;
		  case 'date-desc':
			mSortArray.sort((a, b) => new Date(b.created).getTime() - new Date(a.created).getTime());
			break;
		  case 'size-asc':
			mSortArray.sort((a, b) => this._FileManagerSvc.convertSizeToBytes(a.size) - this._FileManagerSvc.convertSizeToBytes(b.size));
			break;
		  case 'size-desc':
			mSortArray.sort((a, b) => this._FileManagerSvc.convertSizeToBytes(b.size) - this._FileManagerSvc.convertSizeToBytes(a.size));
			break;
		}
		this.files = mSortArray;
	}

	/**
	 * Handle "Select All" checkbox click event
	 * @param {Event} event - description of parameter
	 * @return {void} description of return value
	 */
	onTggleSelectAll(event: Event) {
		const isChecked = (event.target as HTMLInputElement).checked;
		this.selectUnselectText = isChecked ? 'Unselect' : 'Select';
		this.data$().forEach(file => file.selected = isChecked);
	}
}

