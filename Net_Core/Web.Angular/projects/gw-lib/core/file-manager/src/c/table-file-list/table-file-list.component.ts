import {
	AfterViewInit,
	Component,
	effect,
	OnDestroy,
	OnInit,
	TemplateRef,
	ViewChild
} from '@angular/core';
import {
	FormBuilder,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { debounceTime, Subject, Subscription, switchMap } from 'rxjs';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
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

		MatButtonModule,
		MatFormFieldModule,
		MatIconModule,
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
	private _FilterSubject = new Subject<string>();
	private _ModalId_Delete = 'FileListComponent.onMenuDeleteClick';
	private _ModalId_Properties = 'FileListComponent.onMenuPropertiesClick';
	private _ModalId_Rename: string = 'FileListComponent.onRenameClick';
	private _Subscription: Subscription = new Subscription();

	dataSource = new MatTableDataSource<IFileInfoLight>([]);
	// displayedColumns pulled from IFileInfoLight Note: order here effects the order in the table
	displayedColumns: string[] = [
		'selected',
		'fullName',
		'createdShort',
		'modifiedShort',
		'size'
	];
	id: string = '';
	filterTerm: string = '';
	frmRenameFile!: FormGroup;
	menuTopLeftPosition = { x: '0', y: '0' }; // we create an object that contains coordinates 
	selectedFile!: IFileInfoLight;
	selectUnselectText: string = 'Check';
	showDelete: boolean = false;
	showDownload: boolean = false;
	showRename: boolean = false;

	@ViewChild(MatPaginator) paginator!: MatPaginator;
	@ViewChild(MatSort) sort!: MatSort;
	// reference to the MatMenuTrigger in the DOM 
	@ViewChild(MatMenuTrigger, { static: true }) private _MatMenuTrigger!: MatMenuTrigger;
	@ViewChild('deleteFile', { read: TemplateRef }) private _DeleteFile!: TemplateRef<unknown>;
	@ViewChild('deleteSelected', { read: TemplateRef }) private _DeleteSelected!: TemplateRef<unknown>;
	@ViewChild('fileProperties', { read: TemplateRef }) private _FileProperties!: TemplateRef<unknown>;
	@ViewChild('renameFile', { read: TemplateRef }) private _RenameFile!: TemplateRef<unknown>;

	constructor(
		private _FileManagerSvc: FileManagerService,
		private _FormBuilder: FormBuilder,
		private _GWCommon: GWCommon,
		private _LoggingSvc: LoggingService,
		private _ModalSvc: ModalService,
		private _Router: Router,
		private _SecuritySvc: SecurityService
	) {
		effect(() => {
			this.dataSource.data = this._FileManagerSvc.fileInfoList$();
		});
	}

	ngOnDestroy(): void {
		this._Action = '';
		this._Subscription.unsubscribe();
	}

	ngOnInit(): void {
		this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
		this._FileManagerSvc.setAllSelected(false);
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
			this.dataSource.data = this._FileManagerSvc.fileInfoList$();
		}
		this._Subscription.add(
			this._FilterSubject.pipe(
				debounceTime(300),
				switchMap(filterValue => {
					this.applyFilter(filterValue);
					return [];
				})
			).subscribe()
		);
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
	 * Checks if all the files are selected, and if so, changes the text of the
	 * select/unselect button to 'Unselect'. If not, changes the text to 'Select'.
	 * @returns {boolean} true if all files are selected, false otherwise
	 */
	get allSelected(): boolean {
		if (!this.paginator || this.paginator.pageIndex === undefined) {
			return false; // or true, depending on your default behavior
		}
		const startIndex = this.paginator.pageIndex * this.paginator.pageSize;
		const endIndex = startIndex + this.paginator.pageSize;
		const visibleRows = this.dataSource.data.slice(startIndex, endIndex); // Get currently visible rows
		return visibleRows.every(row => row.selected); // Check if all visible rows are selected
	}

	/**
	 * Determines if any of the files are selected.
	 *
	 * @returns {boolean} true if any of the files are selected, false otherwise
	 */
	get anySelected(): boolean {
		return this._FileManagerSvc.fileInfoList$().some(file => file.selected);
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
		// Custom sorting logic for size column
		this.dataSource.sortingDataAccessor = (item: IFileInfoLight, property: string) => {
			if (property === 'size') {
				return this._FileManagerSvc.convertSizeToBytes(item.size); // Convert to bytes for sorting
			}
			return item[property as keyof IFileInfoLight].toString(); // Fallback for dynamic properties
		};
		this._Subscription.add(
			this.paginator.page.subscribe(() => {
				this._FileManagerSvc.setAllSelected(false);
			})
		);
	}

	/**
	 * Applies a filter to the data source based on the provided filter value.
	 *
	 * @param {string} filterValue - The value used to filter the data. It is trimmed
	 * and converted to lowercase before being applied.
	 *
	 * Resets the paginator to the first page if it exists.
	 */
	applyFilter(filterValue: string) {
		let mFilterValue = '';
		if (!this._GWCommon.isNullOrEmpty(filterValue)) {
			mFilterValue = filterValue;
		}
		this.dataSource.filter = mFilterValue.trim().toLowerCase();

		if (this.dataSource.paginator) {
			this.dataSource.paginator.firstPage();
		}
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
		const filterValue = (event.target as HTMLInputElement).value;
		this._FilterSubject.next(filterValue);
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
		this._FileManagerSvc.setAllSelected(false);
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
	 * Handle "Select All" checkbox click event
	 * @param {Event} event - description of parameter
	 * @return {void} description of return value
	 */
	onToggleSelectAll(event: Event) {
		const isChecked = (event.target as HTMLInputElement).checked;
		this.selectUnselectText = isChecked ? 'Uncheck' : 'Check';
		// We can not use the service because we are using an Angular Material component
		// and we do not want to have any rows checked if they are not visiable
		// Get the currently visible rows
		const startIndex = this.paginator.pageIndex * this.paginator.pageSize;
		const endIndex = startIndex + this.paginator.pageSize;
		const visibleRows = this.dataSource.data.slice(startIndex, endIndex);
		// Update the selected property of visible rows
		visibleRows.forEach(row => row.selected = isChecked);
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
