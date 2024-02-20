import { Component } from '@angular/core';

import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
// Library
import { ModalService } from '@growthware/core/modal';
import { LoggingService, LogLevel } from '@growthware/core/logging';
// Feature
import { FileManagerService } from '../../file-manager.service';

@Component({
	selector: 'gw-core-add-directory',
	standalone: true,
	imports: [MatButtonModule, ReactiveFormsModule],
	templateUrl: './add-directory.component.html',
	styleUrls: ['./add-directory.component.scss']
})
export class AddDirectoryComponent {
	private _Action: string = '';

	public configurationName: string = '';
	public frmCreateDirectory!: FormGroup;

	constructor(
    private _FileManagerSvc: FileManagerService,
    private _FormBuilder: FormBuilder,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _Router: Router
	) {}

	ngOnInit(): void {
		this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
		this.configurationName = this._Action + '_Directories';
		this.populateCreateDirectoryForm('');
	}

	/**
	 * Returns the controls of the 'frmCreateDirectory' form.
	 *
	 * @return {any} The controls of the 'frmCreateDirectory' form.
	 */
	get getControls() {
		return this.frmCreateDirectory.controls;
	}

	/**
	 * @description Return text for the given field if there is an error with the field
	 *
	 * @param {string} fieldName
	 * @return {*}  {(string | undefined)}
	 * @memberof DirectoryTreeComponent
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
	 * Executes the logic to submit the creation of a directory.
	 *
	 * @param {void} - No parameters needed.
	 * @return {void} - No return value.
	 */
	onCreateDirectorySubmit(): void {
		this._FileManagerSvc.createDirectory(this._Action, this.getControls['newDirectoryName'].value).then((response) => {
			let mMsg = 'Folder has been created';
			let mLogLevel = LogLevel.Success;
			if(response === false) {
				mMsg = 'Folder was NOT created';
				mLogLevel = LogLevel.Error;
			}
			this.frmCreateDirectory.reset();
			this._ModalSvc.close(this._FileManagerSvc.ModalId_CreateDirectory);
			this._LoggingSvc.toast(mMsg, 'New Folder', mLogLevel);					
		}).catch((error) => {
			this._LoggingSvc.errorHandler(error, 'FileManagerComponent', 'onCreateDirectorySubmit');
		});
	}

	/**
	 * Populates the create directory form with the given directory name.
	 *
	 * @param {string} directoryName - The name of the directory to be populated.
	 * @return {void} This function does not return anything.
	 */
	private populateCreateDirectoryForm(directoryName: string): void {
		this.frmCreateDirectory = this._FormBuilder.group({
			newDirectoryName: [directoryName, [Validators.required]],
		});
	}
}
