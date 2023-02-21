import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LogDestination, ILogOptions, LogOptions } from '@Growthware/Lib/src/lib/features/logging';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
// Feature
import { FileManagerService } from '../../file-manager.service';


@Component({
  selector: 'gw-lib-file-manager',
  templateUrl: './file-manager.component.html',
  styleUrls: ['./file-manager.component.scss']
})
export class FileManagerComponent implements OnInit {

  @Input() configurationName: string = '';
  directoryConfigurationName: string = '';

  constructor(
    private _FileManagerSvc: FileManagerService,
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _Router: Router
  ) { 
    // do nothing atm
  }

  ngOnInit(): void {
    const mRouteOnly = this._Router.url.split('?')[0] .replace('/', '').replace('\\','');
    this.configurationName = mRouteOnly;
    this.directoryConfigurationName = this.configurationName + '_Directories';
    this._FileManagerSvc.getDirectories(mRouteOnly, this.directoryConfigurationName);
  }
}
