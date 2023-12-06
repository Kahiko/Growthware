import { Component, Input, OnInit } from '@angular/core';
import { TemplateRef, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
// Library
import { ModalOptions, ModalService, WindowSize } from '@Growthware/features/modal';
// Feature
import { FileManagerService } from '../../file-manager.service';
import { AddDirectoryComponent } from '../add-directory/add-directory.component';

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
  @ViewChild('helpText', { read: TemplateRef }) private _HelpText!:TemplateRef<any>;

  constructor(
    private _FileManagerSvc: FileManagerService,
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
  }

  onCreateDirectory() {
    // console.log('item', item);
    const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_CreateDirectory, 'New Folder', AddDirectoryComponent, new WindowSize(84, 300));
    this._ModalSvc.open(mModalOptions);
  }

  onHelp(): void {
    const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_CreateDirectory, 'Help', this._HelpText, new WindowSize(200, 550));
    this._ModalSvc.open(mModalOptions)
  }

  onRefresh(): void {
    this._FileManagerSvc.refresh(this._Action);
  }
}
