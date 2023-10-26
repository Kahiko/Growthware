import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TemplateRef } from '@angular/core';
import { MatMenuTrigger } from '@angular/material/menu';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
import { DataNVP } from '@Growthware/shared/models';
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
import { ModalOptions, ModalService, WindowSize } from '@Growthware/features/modal';
import { ISecurityInfo } from '@Growthware/features/security';
import { SecurityService } from '@Growthware/features/security';
// Feature
import { FileManagerService } from '../../file-manager.service';
import { IFileInfoLight } from '../../file-info-light.model';

@Component({
  selector: 'gw-lib-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.scss']
})
export class FileListComponent implements OnInit {

  private _Action: string = '';
  private _DataSubject = new BehaviorSubject<any[]>([]);
  private _ModalId_Delete = 'FileListComponent.onMenuDeleteClick';
  private _ModalId_Properties = 'FileListComponent.onMenuPropertiesClick';
  private _ModalId_Rename: string = 'FileListComponent.onRenameClick';
  private _Subscriptions: Subscription = new Subscription();

  readonly data$ = this._DataSubject.asObservable();

  id: string = '';
  frmRenameFile!: FormGroup;
  menuTopLeftPosition =  {x: '0', y: '0'}; // we create an object that contains coordinates 
  selectedFile!: IFileInfoLight;
  showDelete: boolean = false;
  showDownalod: boolean = false;
  showRename: boolean = false;

  @Input() numberOfColumns: string = '4';

  // reference to the MatMenuTrigger in the DOM 
  @ViewChild( MatMenuTrigger, {static: true}) private _MatMenuTrigger!: MatMenuTrigger;
  @ViewChild('deleteFile', { read: TemplateRef }) private _DeleteFile!:TemplateRef<any>;
  @ViewChild('fileProperties', { read: TemplateRef }) private _FileProperties!:TemplateRef<any>;
  @ViewChild('renameFile', { read: TemplateRef }) private _RenameFile!:TemplateRef<any>;

  constructor(
    private _DataSvc: DataService,
    private _GWCommon: GWCommon,
    private _FileManagerSvc: FileManagerService,
    private _FormBuilder: FormBuilder,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _Router: Router,
    private _SecuritySvc: SecurityService
  ) { }

  ngOnDestroy(): void {
    this._Subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this._Action = this._Router.url.split('?')[0] .replace('/', '').replace('\\','');
    this.id = this._Action + "_Files";
    this._SecuritySvc.getSecurityInfo(this._Action).then((response: ISecurityInfo) => {
      this.showDownalod = response.mayView;
      this.showDelete = response.mayDelete;
      this.showRename = response.mayEdit;
    }).catch((error)=>{
      this._LoggingSvc.errorHandler(error, 'FileListComponent', 'ngOnInit');
    });

    if(this._GWCommon.isNullOrUndefined(this.id)) {
      this._LoggingSvc.toast('The id can not be blank!', 'File List Component', LogLevel.Error);
    } else {
      this._Subscriptions.add(this._DataSvc.dataChanged.subscribe((data: DataNVP) => {
        if(data.name.toLocaleLowerCase() === this.id.toLowerCase()) {
          this._DataSubject.next(data.payLoad);
        }
      }));
    }
  }

  get getControls() {
    return this.frmRenameFile.controls;
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

  getTemplateColumnsStyle(): Object {
    let obj :Object;
    obj = { 'grid-template-columns':'repeat('+Number(this.numberOfColumns)+', 1fr)'};
    // console.log('obj', obj);
    return obj;
  }

  onLeftClick(item: IFileInfoLight) {
    if(!this.selectedFile) {
      this.selectedFile = item;
    } else {
      if(this.selectedFile.name !== item.name) {
        this.selectedFile = item;
      } else {
        this.selectedFile.name = '';
      }  
    }
  }

  onMenuDeleteClick(item: IFileInfoLight){
    // console.log('item', item);
    this.selectedFile = item;
    const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Delete, 'Delete File', this._DeleteFile, new WindowSize(84, 300));
    mModalOptions.buttons.okButton.visible = true;
    mModalOptions.buttons.okButton.text = 'Yes';
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this._FileManagerSvc.deleteFile(this._Action, this.selectedFile.name).then((_)=>{
        this._FileManagerSvc.getFiles(this._Action, this.id, this._FileManagerSvc.SelectedPath);
        this._LoggingSvc.toast('File was deleted', 'Delete file', LogLevel.Success);
      }).catch((error) => {
        this._LoggingSvc.errorHandler(error, 'FileListComponent', 'onMenuDeleteClick');
        this._LoggingSvc.toast('Was not able to delete the file', 'Delete file error', LogLevel.Error);
      });
      this._ModalSvc.close(this._ModalId_Delete);
    }
    this._ModalSvc.open(mModalOptions);
  }

  onMenuDownloadClick(item: IFileInfoLight) {
    // console.log('item', item);
    this.selectedFile = item;
    this._FileManagerSvc.getFile(this._Action, this._FileManagerSvc.SelectedPath, item.name);
  }

  onMenuRenameClick(item: IFileInfoLight) {
    // console.log('item', item);
    this.selectedFile = item;
    this.populateRenameFileForm(this.selectedFile.name);
    const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Rename, 'Rename File', this._RenameFile, new WindowSize(84, 300));
    this._ModalSvc.open(mModalOptions);
  }

  onMenuPropertiesClick(item: IFileInfoLight) {
    this.selectedFile = item;
    const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Properties, 'Properties', this._FileProperties, new WindowSize(80, 600));
    mModalOptions.buttons.okButton.visible = true;
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this._ModalSvc.close(this._ModalId_Properties);
    }
    this._ModalSvc.open(mModalOptions);
  }

  onRenameSubmit(form: FormGroup): void {
    this._FileManagerSvc.renameFile(this._Action, this.selectedFile.name, form.value['newFileName']).then((response) => {
      form.reset();
      this._FileManagerSvc.getFiles(this._Action, this.id, this._FileManagerSvc.SelectedPath);
      this._ModalSvc.close(this._ModalId_Rename);
      this._LoggingSvc.toast('File was renamed', 'Rename file', LogLevel.Success);
    }).catch((error) => {
      this._LoggingSvc.errorHandler(error, 'FileListComponent', 'onRenameSubmit');
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
  onRightClick(event: MouseEvent, item: any) {
    this.selectedFile = item.content;
    // preventDefault avoids to show the visualization of the right-click menu of the browser
    event.preventDefault();

    // we record the mouse position in our object
    this.menuTopLeftPosition.x = event.clientX.toString();
    this.menuTopLeftPosition.y = event.clientY.toString();

    // we open the menu
    // we pass to the menu the information about our object
    this._MatMenuTrigger.menuData = {item: item}

    // we open the menu
    this._MatMenuTrigger.openMenu();
  }
}

