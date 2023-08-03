import { Component, Input, OnInit } from '@angular/core';
import { TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
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

  private _Action: string = '';
  private _ModalId_CreateDirectory: string = "CreateDirectoryForm";
  private _Skin: BehaviorSubject<string> = new BehaviorSubject<string>('default');

  fileContent: string = 'snake'
  frmCreateDirectory!: FormGroup;
  validFileContents = [
    {id: 'snake', name: 'List', icon: 'summarize'},
    {id: 'details', name: 'Details', icon: 'list'},
    {id: 'table', name: 'Table', icon: 'table_rows'},
  ];
  readonly skin$ = this._Skin.asObservable();

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
    this._Action = mAction;
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

  onCreateDirectorySubmit(): void
  {
    // this.frmCreateDirectory.reset();
    // this._ModalSvc.close(this._ModalId_CreateDirectory);
    this._FileManagerSvc.createDirectory(this._Action, this.getControls['newDirectoryName'].value).then((response) => {
      this.frmCreateDirectory.reset();
      this._ModalSvc.close(this._ModalId_CreateDirectory);
      this._LoggingSvc.toast('Folder has been created', 'New Folder', LogLevel.Success);
    }).catch((error) => {
      this._LoggingSvc.errorHandler(error, 'FileManagerComponent', 'onCreateDirectorySubmit');
    });
  }


  private populateCreateDirectoryForm(directoryName: string): void {
    this.frmCreateDirectory = this._FormBuilder.group({
      newDirectoryName: [directoryName, [Validators.required]],
    });
  }

  onRefresh(): void {
    this._FileManagerSvc.refresh(this._Action);
  }
}
