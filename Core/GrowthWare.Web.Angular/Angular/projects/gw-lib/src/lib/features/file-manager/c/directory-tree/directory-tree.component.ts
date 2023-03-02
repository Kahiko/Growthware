import { Component, Input, AfterViewInit, OnInit } from '@angular/core';
import { ViewChild } from '@angular/core';
import { NestedTreeControl } from '@angular/cdk/tree';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TemplateRef } from '@angular/core';
import { MatMenuTrigger } from '@angular/material/menu';
// import { MatTreeNestedDataSource } from '@angular/material/tree';
import { Router } from '@angular/router';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
import { DataNVP } from '@Growthware/Lib/src/lib/models';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LogDestination, ILogOptions, LogOptions } from '@Growthware/Lib/src/lib/features/logging';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { ModalOptions, ModalService, WindowSize } from '@Growthware/Lib/src/lib/features/modal';
// Feature
import { FileManagerService } from '../../file-manager.service';
import { IDirectoryTree } from '../../directory-tree.model';

@Component({
  selector: 'gw-lib-directory-tree',
  templateUrl: './directory-tree.component.html',
  styleUrls: ['./directory-tree.component.scss']
})
export class DirectoryTreeComponent implements AfterViewInit, OnInit {

  private _DataSubject = new BehaviorSubject<IDirectoryTree[]>([]);
  private _Subscriptions: Subscription = new Subscription();

  @Input() doGetFiles: boolean = true;

  activeNode?: IDirectoryTree;
  configurationName: string = ''
  frmRenameDirectory!: FormGroup;
  menuTopLeftPosition =  {x: '0', y: '0'}; // we create an object that contains coordinates 

  get getControls() {
    return this.frmRenameDirectory.controls;
  }

  treeControl = new NestedTreeControl<IDirectoryTree>(node => node.children);
  readonly dataSource = this._DataSubject.asObservable();
  // reference to the MatMenuTrigger in the DOM 
  @ViewChild( MatMenuTrigger, {static: true}) private _MatMenuTrigger!: MatMenuTrigger;
  @ViewChild('deleteDirectory', { read: TemplateRef }) private _DeleteDirectory!:TemplateRef<any>;
  @ViewChild('directoryProperties', { read: TemplateRef }) private __DirectoryProperties!:TemplateRef<any>;
  @ViewChild('renameDirectory', { read: TemplateRef }) private _RenameDirectory!:TemplateRef<any>;


  constructor(
    private _DataSvc: DataService,
    private _FileManagerSvc: FileManagerService,
    private _GWCommon: GWCommon,
    private _FormBuilder: FormBuilder,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _Router: Router
  ) { 
    // do nothing
  }

  ngAfterViewInit(): void {

  }

  ngOnInit(): void {
    this.configurationName = this._Router.url.split('?')[0] .replace('/', '').replace('\\','') + '_Directories';
    if(!this._GWCommon.isNullOrEmpty(this.configurationName)) {
      this.populateRenameDirectoryForm()
      // logic to start getting data
      this._Subscriptions.add(this._DataSvc.dataChanged.subscribe((data: DataNVP) => {
        if(data.name.toLowerCase() === this.configurationName.toLowerCase()) {
          // console.log('data.payLoad', data.payLoad);
          this._DataSubject.next(data.payLoad);
          if(this.doGetFiles) {
            const mAction = this.configurationName.replace('_Directories', '');
            const mForControlName = mAction + '_Files';
            this.activeNode = data.payLoad[0];
            this._FileManagerSvc.getFiles(mAction, mForControlName, data.payLoad[0].relitivePath);
          }
        }
      }));
    } else {
      const mLogDestinations: Array<LogDestination> = [];
      mLogDestinations.push(LogDestination.Console);
      mLogDestinations.push(LogDestination.Toast);
      const mLogOptions: ILogOptions = new LogOptions(
        'DirectoryTreeComponent.ngOnInit: configurationName is blank',
        LogLevel.Error,
        mLogDestinations,
        'DirectoryTreeComponent',
        'DirectoryTreeComponent',
        'ngOnInit',
        'system',
        'DirectoryTreeComponent'
      )
      this._LoggingSvc.log(mLogOptions);
    }
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

  hasChild = (_: number, node: IDirectoryTree) => !!node.children && node.children.length > 0;


  onMenuDeleteClick(item: IDirectoryTree){
    console.log('item', item);
    const mModalOptions: ModalOptions = new ModalOptions('DirectoryTreeComponent.onMenuDeleteClick', 'Delete Directory', this._DeleteDirectory, new WindowSize(84, 300));
    mModalOptions.buttons.okButton.visible = true;
    mModalOptions.buttons.okButton.text = 'Yes';
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this._ModalSvc.close('FileListComponent.onDeleteClick');
    }
    this._ModalSvc.open(mModalOptions);
  }

  onMenuPropertiesClick(item: IDirectoryTree) {
    const mModalOptions: ModalOptions = new ModalOptions('DirectoryTreeComponent.onMenuPropertiesClick', 'Properties', this.__DirectoryProperties, new WindowSize(80, 600));
    mModalOptions.buttons.okButton.visible = true;
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this._ModalSvc.close('FileListComponent.onPropertiesClick');
    }
    this._ModalSvc.open(mModalOptions);
  }

  onMenuRenameClick(item: IDirectoryTree) {
    console.log('item', item);
    const mModalOptions: ModalOptions = new ModalOptions('DirectoryTreeComponent.onMenuRenameClick', 'Rename File', this._RenameDirectory, new WindowSize(84, 300));
    this._ModalSvc.open(mModalOptions);
  }

  onRenameSubmit(form: FormGroup): void {

  }

  /**
   * Method called when the user click with the right button
   * @param event MouseEvent, it contains the coordinates
   * @param item Our data contained in the row of the table
   */
  onRightClick(event: MouseEvent, item: any) {
    console.log('item', item);
    // preventDefault avoids to show the visualization of the right-click menu of the browser
    event.preventDefault();

    // we record the mouse position in our object
    this.menuTopLeftPosition.x = event.clientX.toString();
    this.menuTopLeftPosition.y = event.clientY.toString();

    // we open the menu
    // we pass to the menu the information about our object
    this._MatMenuTrigger.menuData = { item: item }

    // we open the menu
    this._MatMenuTrigger.openMenu();
  }

  private populateRenameDirectoryForm(): void {
    this.frmRenameDirectory = this._FormBuilder.group({
      newFileName: ['', [Validators.required]],
    });
  }

  selectDirectory(node: IDirectoryTree): void {
    this.activeNode = node;
    const mAction = this.configurationName.replace('_Directories', '');
    const mForControlName = mAction + '_Files';
    this._FileManagerSvc.getFiles(mAction, mForControlName, node.relitivePath);
  }
}
