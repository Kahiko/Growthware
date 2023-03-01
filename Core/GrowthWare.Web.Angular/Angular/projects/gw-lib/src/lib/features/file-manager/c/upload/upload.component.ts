import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { TemplateRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LogDestination, ILogOptions, LogOptions } from '@Growthware/Lib/src/lib/features/logging';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { ModalOptions, ModalService, WindowSize } from '@Growthware/Lib/src/lib/features/modal';
// Feature
import { FileManagerService } from '../../file-manager.service';
import { IUploadStatus } from '../../upload-status.model';

@Component({
  selector: 'gw-lib-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnDestroy, OnInit {
  private _NumberOfFilesCompleted: number = 0;  
  private _ProgressModalId: string = 'progressTemplate'
  private _TotalNumberOfFiles: number = 0;
  private _Subscription: Subscription = new Subscription();

  id: string = '';
  isMultiple: boolean = true;
  fileProgressSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  currentFile: string = '';
  overallProgressSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  showFileProgress: boolean = false;
  showOverallProgress: boolean = false;

  @Input() multiple: string = 'false';
  @ViewChild('progressTemplate', { read: TemplateRef }) private _ProgressTemplate!:TemplateRef<any>;

  constructor(
    private _FileManagerSvc: FileManagerService,
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _Router: Router
  ) { }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
    this.id = this._Router.url.split('?')[0] .replace('/', '').replace('\\','') + "_Upload";
    if(!this._GWCommon.isNullOrEmpty(this.id)) {
      this._Subscription.add(this._FileManagerSvc.uploadStatusChanged.subscribe((data: IUploadStatus) => {
        // TODO: the component will need to add the _Files to the name and not have
        // the file-manager.component.ts do this
        // console.log('data', data);
        if (data.id.toLowerCase() + "_upload" === this.id.toLowerCase()) {
          const mPercent: number = Math.floor((data.uploadNumber / data.totalNumberOfUploads) * 100);
          this.fileProgressSubject.next(mPercent);
          if(mPercent == 100) {
            this._NumberOfFilesCompleted = this._NumberOfFilesCompleted + 1;
            const mTotalPercent: number = Math.floor((this._NumberOfFilesCompleted / this._TotalNumberOfFiles) * 100);
            this.overallProgressSubject.next(mTotalPercent);
            if(mTotalPercent == 100) {
              this._GWCommon.sleep(500).then(() => {
                this.showFileProgress = false;
                const mAction = this.id.replace('_Upload', '');
                let mForControlName = mAction + "_Files";
                this._FileManagerSvc.getFiles(mAction, mForControlName, this._FileManagerSvc.SelectedPath);
                mForControlName = mForControlName.replace('_Files', '_Directories');
                this._FileManagerSvc.getDirectories(mAction, '\\', mForControlName);
                this._GWCommon.sleep(3000).then(() => {
                  this.onOk();
                });
              });
            }
          }
        }
      }));
    } else{
      const mLogDestinations: Array<LogDestination> = [];
      mLogDestinations.push(LogDestination.Console);
      mLogDestinations.push(LogDestination.Toast);
      const mLogOptions: ILogOptions = new LogOptions(
        'UploadComponent.ngOnInit: id is blank',
        LogLevel.Error,
        mLogDestinations,
        'UploadComponent',
        'UploadComponent',
        'ngOnInit',
        'system',
        'UploadComponent'
      )
      this._LoggingSvc.log(mLogOptions);
    }
  }

  onFileSelected(event: any): void {  
    // const mFileList: FileList = {...event.target.files};
    const mFileList = event.target.files;
    const mModalOptions: ModalOptions = new ModalOptions(this._ProgressModalId, 'Progress', this._ProgressTemplate, new WindowSize(150, 400));
    this._ModalSvc.open(mModalOptions);
    this._NumberOfFilesCompleted = 0;
    this._TotalNumberOfFiles = mFileList.length;
    this.fileProgressSubject.next(0);
    this.overallProgressSubject.next(0);
    this.showFileProgress = true;
    this.showOverallProgress = true;
    const mAction = this.id.replace('_Upload', '');
    // Loop through all of the selected files and upload them
    for (let index = 0; index < mFileList.length; index++) {
      const mFile = mFileList[index];
      let mFileName: string = mFile.name;
      mFileName = mFileName.substring(0, 50);
      this.currentFile = mFileName;
      this._FileManagerSvc.uploadFile(mAction, mFile);
    }
    // Clear the value so you can choose the same file(s) again
    event.target.value = '';
  }

  onOk(): void {
    this._ModalSvc.close(this._ProgressModalId);
  }
}
