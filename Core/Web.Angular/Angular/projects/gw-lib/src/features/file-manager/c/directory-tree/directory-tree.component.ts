import { Component, Input, AfterViewInit, OnInit } from '@angular/core';
import { ViewChild } from '@angular/core';
import { NestedTreeControl } from '@angular/cdk/tree';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TemplateRef } from '@angular/core';
import { MatMenuTrigger } from '@angular/material/menu';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
// Library
import { DataNVP } from '@Growthware/Lib/src/models';
import { DataService } from '@Growthware/Lib/src/services';
import { GWCommon } from '@Growthware/Lib/src/common-code';
import { LogDestination, ILogOptions, LogOptions } from '@Growthware/Lib/src/features/logging';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/features/logging';
import { ModalOptions, ModalService, WindowSize } from '@Growthware/Lib/src/features/modal';
import { ISecurityInfo, SecurityInfo } from '@Growthware/Lib/src/features/security';
import { SecurityService } from '@Growthware/Lib/src/features/security';
// Feature
import { IDirectoryTree } from '../../directory-tree.model';
import { FileManagerService } from '../../file-manager.service';

@Component({
  selector: 'gw-lib-directory-tree',
  templateUrl: './directory-tree.component.html',
  styleUrls: ['./directory-tree.component.scss']
})
export class DirectoryTreeComponent implements AfterViewInit, OnInit {

  private _Action: string = '';
  private _ModalId_Delete = 'DirectoryTreeComponent.onMenuDeleteClick';
  private _ModalId_Properties = 'DirectoryTreeComponent.onMenuPropertiesClick';
  private _ModalId_Rename = 'DirectoryTreeComponent.onMenuRenameClick';
  private _SecurityInfo: ISecurityInfo = new SecurityInfo();
  private _Subscriptions: Subscription = new Subscription();

  @Input() doGetFiles: boolean = true;

  activeNode?: IDirectoryTree;
  configurationName: string = ''
  frmRenameDirectory!: FormGroup;
  menuTopLeftPosition =  {x: '0', y: '0'}; // we create an object that contains coordinates
  selectedPath: string = '';

  get getControls() {
    return this.frmRenameDirectory.controls;
  }

  dataSource = new MatTreeNestedDataSource<IDirectoryTree>();
  treeControl = new NestedTreeControl<IDirectoryTree>(node => node.children);
  showDelete: boolean = false;
  showRename: boolean = false;

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
    private _Router: Router,
    private _SecuritySvc: SecurityService
  ) { 
    // do nothing
  }

  ngAfterViewInit(): void {

  }

  ngOnInit(): void {
    this._Action = this._Router.url.split('?')[0] .replace('/', '').replace('\\','');
    this.configurationName = this._Action + '_Directories';
    if(!this._GWCommon.isNullOrEmpty(this.configurationName)) {
      this._SecuritySvc.getSecurityInfo(this._Action).then((response: ISecurityInfo) => {
        this._SecurityInfo = response;
      }).catch((error)=>{
        this._LoggingSvc.errorHandler(error, 'FileListComponent', 'ngOnInit');
      });
      this.populateRenameDirectoryForm()
      // logic to start getting data
      this._Subscriptions.add(this._DataSvc.dataChanged.subscribe((data: DataNVP) => {
        if(data.name.toLowerCase() === this.configurationName.toLowerCase()) {
          // console.log('data.payLoad', data.payLoad);
          this.dataSource.data = data.payLoad;
          if(this.doGetFiles) {
            const mAction = this.configurationName.replace('_Directories', '');
            const mForControlName = mAction + '_Files';
            this.activeNode = data.payLoad[0];
            this.selectedPath = data.payLoad[0].relitivePath;
            this._FileManagerSvc.getFiles(mAction, mForControlName, data.payLoad[0].relitivePath);
          }
        }
      }));
      this._Subscriptions.add(this._FileManagerSvc.selectedDirectoryChanged.subscribe((data: IDirectoryTree) => {
        this.expand(this.dataSource.data, data.relitivePath);
        this.selectedPath = data.relitivePath;
        this.activeNode = data;   
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

  /**
   * Expands all the parent nodes given the hierarchical data and the value of the unique node property
   *
   * @param {IDirectoryTree[]} data
   * @param {string} relitivePath
   * @return {*}  {*}
   * @memberof DirectoryTreeComponent
   */
  expand(data: IDirectoryTree[], relitivePath: string): any {
    data.forEach(node => {
      if (node.children && node.children.find(c => c.relitivePath === relitivePath)) {
        this.treeControl.expand(node);
        // this.expand(this.dataSource.data, node.relitivePath);
        this.expand(data, node.relitivePath);
      }
      else if (node.children && node.children.find(c => c.children)) {
        this.expand(node.children, relitivePath);
      }
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

  hasChild = (_: number, node: IDirectoryTree) => !!node.children && node.children.length > 0;

  /**
   * @description Handles the delete menu click
   *
   * @param {IDirectoryTree} item The tree node
   * @memberof DirectoryTreeComponent
   */
  onMenuDeleteClick(item: IDirectoryTree){
    // console.log('item', item);
    const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Delete, 'Delete Directory', this._DeleteDirectory, new WindowSize(84, 300));
    mModalOptions.buttons.okButton.visible = true;
    mModalOptions.buttons.okButton.text = 'Yes';
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this._FileManagerSvc.deleteDirectory(this._Action, this.selectedPath).then((response) => {
        const mPreviousRelitavePath = this.previousRelitavePath(item);
        this._FileManagerSvc.getDirectories(this._Action, mPreviousRelitavePath, this.configurationName).then((response) => {
          const mPreviousDirectoryNode: IDirectoryTree = <IDirectoryTree>this._GWCommon.hierarchySearch(this.dataSource.data, mPreviousRelitavePath, 'relitivePath', 'children');
          this.selectDirectory(mPreviousDirectoryNode);
          this._ModalSvc.close(this._ModalId_Delete);
        }).catch((error: any) => {
          this._LoggingSvc.errorHandler(error, 'DirectoryTreeComponent', 'onMenuDeleteClick');
        });
      }).catch((error) => {
        this._LoggingSvc.errorHandler(error, 'DirectoryTreeComponent', 'onMenuDeleteClick');
        this._LoggingSvc.toast('Was not able to delete the directory', 'Delete directory error', LogLevel.Error);
      });
    }
    this._ModalSvc.open(mModalOptions);
  }

  /**
   * @description Handles the properties menu click
   *
   * @param {IDirectoryTree} item The tree node
   * @memberof DirectoryTreeComponent
   */
  onMenuPropertiesClick(item: IDirectoryTree) {
    // console.log('item', item);
    const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Properties, 'Properties', this.__DirectoryProperties, new WindowSize(80, 600));
    mModalOptions.buttons.okButton.visible = true;
    mModalOptions.buttons.okButton.callbackMethod = () => {
      this._ModalSvc.close(this._ModalId_Properties);
    }
    this._ModalSvc.open(mModalOptions);
  }

  /**
   * @description Handles the rename menu click
   *
   * @param {IDirectoryTree} item The tree node
   * @memberof DirectoryTreeComponent
   */
  onMenuRenameClick(item: IDirectoryTree) {
    // console.log('item', item);
    const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_Rename, 'Rename Directory', this._RenameDirectory, new WindowSize(84, 300));
    this._ModalSvc.open(mModalOptions);
  }

  /**
   * @description Handles the rename submit click
   *
   * @param {IDirectoryTree} item The tree node
   * @memberof DirectoryTreeComponent
   */
  onRenameSubmit(form: FormGroup): void {
    // console.log('form', form);
    this._ModalSvc.close(this._ModalId_Rename);
      this._FileManagerSvc.renameDirectory(this._Action, form.value['newDirectoryName']).then((response) => {
        form.reset();
        this._FileManagerSvc.getFiles(this._Action, this._Action + '_Files', this._FileManagerSvc.SelectedPath);
        this._ModalSvc.close(this._ModalId_Rename);
        this._LoggingSvc.toast('File was renamed', 'Rename direcory', LogLevel.Success);
      }).catch((error) => {
        this._LoggingSvc.errorHandler(error, 'FileListComponent', 'onRenameSubmit');
      });
  }

  /**
   * Method called when the user click with the right button
   * @param event MouseEvent, it contains the coordinates
   * @param item Our data contained in the row of the table
   */
  onRightClick(event: MouseEvent, item: any) {
    // console.log('item.content', item.content);
    this.onSelectDirectory(item.content);
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
   * @description Handles the when the tree node has been clicked
   *
   * @param {IDirectoryTree} node
   * @memberof DirectoryTreeComponent
   */
  onSelectDirectory(node: IDirectoryTree): void {
    this.selectDirectory(node);
  }

  /**
   * @description Return the previous directory node's relitive path
   *
   * @private
   * @param {IDirectoryTree} directoryTree
   * @return {*}  {string}
   * @memberof DirectoryTreeComponent
   */
  private previousRelitavePath(directoryTree: IDirectoryTree): string {
    const mRelitivePathParts = directoryTree.relitivePath.split('\\');
    let mRetVal: string = '';
    for (let index = 1; index < mRelitivePathParts.length -1; index++) {
      mRetVal += '\\' + mRelitivePathParts[index];            
    }
    return mRetVal;
  }

  /**
   * @description Performs the logic for a select directory event
   *
   * @private
   * @param {IDirectoryTree} directoryTree
   * @memberof DirectoryTreeComponent
   */
  private selectDirectory(directoryTree: IDirectoryTree): void {
    this.showDelete = false;
    this.showRename = false;
    if(directoryTree.relitivePath.length !== 0 ) {
      this.showDelete = this._SecurityInfo.mayDelete;
      this.showRename = this._SecurityInfo.mayEdit;
    }
    this._FileManagerSvc.setSelectedDirectory(directoryTree);    
    if(this.doGetFiles) {
      const mAction = this.configurationName.replace('_Directories', '');
      const mForControlName = mAction + '_Files';
      this._FileManagerSvc.getFiles(mAction, mForControlName, directoryTree.relitivePath);
    }
  }
}
