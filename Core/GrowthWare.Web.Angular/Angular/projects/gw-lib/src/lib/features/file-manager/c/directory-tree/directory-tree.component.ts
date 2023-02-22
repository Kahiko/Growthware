import { Component, Input, AfterViewInit, OnInit } from '@angular/core';
import { NestedTreeControl } from '@angular/cdk/tree';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
import { DataNVP } from '@Growthware/Lib/src/lib/models';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LogDestination, ILogOptions, LogOptions } from '@Growthware/Lib/src/lib/features/logging';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';

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

  @Input() configurationName: string = '';
  @Input() filesConfigurationName: string = '';

  activeNode?: IDirectoryTree;
  treeControl = new NestedTreeControl<IDirectoryTree>(node => node.children);
  readonly dataSource = this._DataSubject.asObservable();

  constructor(
    private _DataSvc: DataService,
    private _FileManagerSvc: FileManagerService,
    private _LoggingSvc: LoggingService,
    private _GWCommon: GWCommon
  ) { 
    // do nothing
  }

  ngAfterViewInit(): void {

  }

  ngOnInit(): void {
    this.configurationName = this.configurationName.trim();
    if(!this._GWCommon.isNullOrUndefined(this.configurationName)) {
      // logic to start getting data
      this._Subscriptions.add(this._DataSvc.dataChanged.subscribe((data: DataNVP) => {
        if(data.name.toLowerCase() === this.configurationName.toLowerCase()) {
          this._DataSubject.next(data.payLoad);
          const mAction = this.configurationName.replace('_Directories', '');
          const mForControlName = mAction + '_Files';
          this.activeNode = data.payLoad[0];
          this._FileManagerSvc.getFiles(mAction, mForControlName, data.payLoad[0].name);
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

  hasChild = (_: number, node: IDirectoryTree) => !!node.children && node.children.length > 0;

  selectDirectory(node: any): void {
    this.activeNode = node;
    const mAction = this.configurationName.replace('_Directories', '');
    const mForControlName = mAction + '_Files';
    this._FileManagerSvc.getFiles(mAction, mForControlName, node.name);
  }
}
