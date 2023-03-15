import { Component, Input, OnInit } from '@angular/core';
import { TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LogDestination, ILogOptions, LogOptions } from '@Growthware/Lib/src/lib/features/logging';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { ModalOptions, ModalService, WindowSize } from '@Growthware/Lib/src/lib/features/modal';
// Feature
import { FileManagerService } from '../../file-manager.service';


@Component({
  selector: 'gw-lib-file-manager',
  templateUrl: './file-manager.component.html',
  styleUrls: ['./file-manager.component.scss']
})
export class FileManagerComponent implements OnInit {

  private _ModalId_CreateDirectory: string = "CreateDirectoryForm";

  frmCreateDirectory!: FormGroup;

  @Input() configurationName: string = '';
  @ViewChild('createDirectory', { read: TemplateRef }) private _CreateDirectory!:TemplateRef<any>;

  constructor(
    private _FileManagerSvc: FileManagerService,
    private _GWCommon: GWCommon,
    private _FormBuilder: FormBuilder,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _Router: Router
  ) { 
    // do nothing atm
  }

  ngOnInit(): void {
    const mAction: string = this._Router.url.split('?')[0] .replace('/', '').replace('\\','');
    this.configurationName = mAction;
    const mForControl: string = mAction + "_Directories";
    this._FileManagerSvc.getDirectories(mAction, '\\', mForControl);
    this.populateCreateDirectoryForm('');
  }

  get getControls() {
    return this.frmCreateDirectory.controls;
  }

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


  onCreateDirectory() {
    // console.log('item', item);
    const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_CreateDirectory, 'New Folder', this._CreateDirectory, new WindowSize(84, 300));
    this._ModalSvc.open(mModalOptions);
  }

  onCreateDirectorySubmit(form: FormGroup): void
  {
    this._ModalSvc.close(this._ModalId_CreateDirectory);
  }


  private populateCreateDirectoryForm(directoryName: string): void {
    this.frmCreateDirectory = this._FormBuilder.group({
      newDirectoryName: [directoryName, [Validators.required]],
    });
  }

}
