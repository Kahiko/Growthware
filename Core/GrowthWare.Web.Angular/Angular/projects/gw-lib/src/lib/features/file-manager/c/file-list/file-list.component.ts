import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TemplateRef } from '@angular/core';
import { MatMenuTrigger } from '@angular/material/menu';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
import { DataNVP } from '@Growthware/Lib/src/lib/models';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { ModalOptions, ModalService, WindowSize } from '@Growthware/Lib/src/lib/features/modal';
import { ISecurityInfo, SecurityInfo } from '@Growthware/Lib/src/lib/models';
import { SecurityService } from '@Growthware/Lib/src/lib/services';
// Feature
import { IFileInfoLight } from '../../file-info-light.model';

@Component({
  selector: 'gw-lib-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.scss']
})
export class FileListComponent implements OnInit {

  private _DataSubject = new BehaviorSubject<any[]>([]);
  private _Subscriptions: Subscription = new Subscription();

  readonly data = this._DataSubject.asObservable();

  id: string = '';
  frmRenameFile!: FormGroup;
  menuTopLeftPosition =  {x: '0', y: '0'}; // we create an object that contains coordinates 
  selectedFile!: IFileInfoLight;
  showDelete: boolean = false;
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
    const mAction = this._Router.url.split('?')[0] .replace('/', '').replace('\\','');
    this.id = mAction + "_Files";
    this._SecuritySvc.getSecurityInfo(mAction).then((response: ISecurityInfo) => {
      this.showDelete = response.mayDelete;
      this.showRename = response.mayEdit;
    }).catch((error)=>{
      this._LoggingSvc.errorHandler(error, 'FileListComponent', 'ngOnInit');
    });


    this.populateRenameFileForm();
    if(this._GWCommon.isNullOrUndefined(this.id)) {
      this._LoggingSvc.toast('The is can not be blank!', 'File List Component', LogLevel.Error);
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

  onMenuDeleteClick(item: IFileInfoLight){
    console.log('item', item);
    this.selectedFile = item;
    const mModalOptions: ModalOptions = new ModalOptions('FileListComponent.onDeleteClick', 'Delete File', this._DeleteFile, new WindowSize(84, 300));
    mModalOptions.buttons.okButton.visible = true;
    mModalOptions.buttons.okButton.text = 'Yes';
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this._ModalSvc.close('FileListComponent.onDeleteClick');
    }
    this._ModalSvc.open(mModalOptions);
  }

  onMenuRenameClick(item: IFileInfoLight) {
    console.log('item', item);
    this.selectedFile = item;
    const mModalOptions: ModalOptions = new ModalOptions('FileListComponent.onRenameClick', 'Rename File', this._RenameFile, new WindowSize(84, 300));
    this._ModalSvc.open(mModalOptions);
  }

  onMenuPropertiesClick(item: IFileInfoLight) {
    this.selectedFile = item;
    const mModalOptions: ModalOptions = new ModalOptions('FileListComponent.onPropertiesClick', 'Properties', this._FileProperties, new WindowSize(80, 600));
    mModalOptions.buttons.okButton.visible = true;
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this._ModalSvc.close('FileListComponent.onPropertiesClick');
    }
    this._ModalSvc.open(mModalOptions);
  }

  onRenameSubmit(form: FormGroup): void {

  }

  private populateRenameFileForm(): void {
    this.frmRenameFile = this._FormBuilder.group({
      newFileName: ['', [Validators.required]],
    });
  }

  /**
   * Method called when the user click with the right button
   * @param event MouseEvent, it contains the coordinates
   * @param item Our data contained in the row of the table
   */
  onRightClick(event: MouseEvent, item: any) {
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

