import { Component, OnInit } from '@angular/core';

import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
// Library
import { ModalService } from '@growthware/core/modal';
import { LoggingService, LogLevel } from '@growthware/core/logging';
// Feature
import { FileManagerService } from '../../file-manager.service';

@Component({
	selector: 'gw-core-rename-directory',
	standalone: true,
	imports: [MatButtonModule, ReactiveFormsModule],
	templateUrl: './rename-directory.component.html',
	styleUrls: ['./rename-directory.component.scss'],
})
export class RenameDirectoryComponent implements OnInit {
	private _Action: string = '';

	public configurationName: string = '';
	public frmRenameDirectory!: FormGroup;

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
		this.populateRenameDirectoryForm();
		// console.log(this._Action);
	}

	get getControls() {
		return this.frmRenameDirectory.controls;
	}

	/**
   * @description populates the Rename Directory Form
   *
   * @private
   * @memberof DirectoryTreeComponent
   */
	private populateRenameDirectoryForm(): void {
		this.frmRenameDirectory = this._FormBuilder.group({
			newDirectoryName: ['', [Validators.required]],
		});
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
   * @description Handles the rename submit click
   *
   * @param {IDirectoryTree} item The tree node
   * @memberof DirectoryTreeComponent
   */
	onRenameSubmit(form: FormGroup): void {
		this._FileManagerSvc.renameDirectory(this._Action, form.value['newDirectoryName'], this._Action + '_Directories').then((response) => {
			form.reset();
			this._ModalSvc.close(this._FileManagerSvc.ModalId_Rename_Directory);
			this._LoggingSvc.toast('The directory has been renamed', 'Rename direcory', LogLevel.Success);
		}).catch((error) => {
			this._LoggingSvc.toast('Directory was NOT renamed', 'Rename direcory', LogLevel.Error);
			this._LoggingSvc.errorHandler(error, 'RenameDirectoryComponent', 'onRenameSubmit');
		});
	}
}
