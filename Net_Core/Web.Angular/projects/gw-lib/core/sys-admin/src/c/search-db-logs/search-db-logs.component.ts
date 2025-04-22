import { Component } from '@angular/core';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
// Library
import { BaseSearchComponent } from '@growthware/core/base/components';
import { DynamicTableService, DynamicTableComponent } from '@growthware/core/dynamic-table';
import { GWCommon } from '@growthware/common/services';
import { LoaderService } from '@growthware/core/loader';
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { ModalService, WindowSize } from '@growthware/core/modal';
import { SearchService } from '@growthware/core/search';
// Feature
import { DBLogDetailsComponent } from '../db-log-details/db-log-details.component';
import { SysAdminService } from '../../sys-admin.service';

@Component({
  selector: 'gw-core-search-db-logs',
  standalone: true,
  imports: [
    DynamicTableComponent,

    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './search-db-logs.component.html',
  styleUrl: './search-db-logs.component.scss'
})
export class SearchDBLogsComponent extends BaseSearchComponent {

  isExporting: boolean = false;

  private _GWCommon: GWCommon;
  private _LoaderSvc: LoaderService;
  private _LoggingSvc: LoggingService;
  private _SysAdminService: SysAdminService;

  constructor(
    theFeatureSvc: SysAdminService,
    dynamicTableSvc: DynamicTableService,
    gwCommon: GWCommon,
    loaderSvc: LoaderService,
    loggingService: LoggingService,
    modalSvc: ModalService,
    searchSvc: SearchService,
  ) {
    super();
    this.configurationName = 'DBLogs';
    this._TheFeatureName = 'DBLogs';
    this._TheApi = 'GrowthwareAPI/SearchDBLogs';
    this._TheComponent = DBLogDetailsComponent;
    this._TheWindowSize = new WindowSize(565, 950);
    this._TheService = theFeatureSvc;
    this._DynamicTableSvc = dynamicTableSvc;
    this._GWCommon = gwCommon;
    this._LoaderSvc = loaderSvc;
    this._LoggingSvc = loggingService;
    this._ModalSvc = modalSvc;
    this._SearchSvc = searchSvc;
    this._SysAdminService = theFeatureSvc;
  }

  /**
   * Handles the "Download all Logs" button click event.
   *
   * @returns {void}
   */
  onDownload(): void {
    try {
      this.isExporting = true;
      this._LoaderSvc.resume();
      this._LoaderSvc.setLoading(true);
      // Call the API to create the system logs zip file
      this._SysAdminService.createSystemLogs().then((response: string) => {
        const mFileId = response;
        // Download the zip file
        this._SysAdminService.downloadSystemLogs(mFileId).then((response: Blob) => {
          const mBlob = new Blob([response], { type: 'text/csv' });
          const mUrl = window.URL.createObjectURL(mBlob);
          const mLink = document.createElement('a');
          mLink.href = mUrl;
          mLink.download = 'SystemLogs.zip';
          mLink.click();

          // Wait for download to complete
          this._GWCommon.waitForDownload(mLink).then(() => {
            // Clean up
            window.URL.revokeObjectURL(mUrl);
            this._SysAdminService.cleanupSystemLogs(mFileId).then(() => {
              this.isExporting = false;
              this._LoaderSvc.setLoading(false);
            });
          });
        }).catch((error) => {
          this._LoggingSvc.console('Error exporting logs:\r\n' + error, LogLevel.Error);
          this._LoggingSvc.toast('Error exporting logs', 'Search DB Logs:', LogLevel.Error);
          this.isExporting = false;
          this._LoaderSvc.setLoading(false);
        });
      }).catch((error: string) => {
        this._LoggingSvc.console('Error exporting logs:\r\n' + error, LogLevel.Error);
        this._LoggingSvc.toast('Error exporting logs', 'Search DB Logs:', LogLevel.Error);
        this.isExporting = false;
        this._LoaderSvc.setLoading(false);
      });
    } catch (error) {
      this._LoggingSvc.console('Error exporting logs:\r\n' + error, LogLevel.Error);
      this._LoggingSvc.toast('Error exporting logs', 'Search DB Logs:', LogLevel.Error);
      this.isExporting = false;
      this._LoaderSvc.setLoading(false);
    }
  }
}
