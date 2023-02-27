import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { TemplateRef, ViewChild } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
// Library
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LogDestination, ILogOptions, LogOptions } from '@Growthware/Lib/src/lib/features/logging';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';
import { ModalOptions, ModalService, WindowSize } from '@Growthware/Lib/src/lib/features/modal';
// Feature
import { FileManagerService } from '../../file-manager.service';

@Component({
  selector: 'gw-lib-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnDestroy, OnInit {
  private _NumberOfFilesCompleted: number = 0;  
  private _ProgressModalId: string = 'progressTemplate'
  private _TotalNumberOfFiles: number = 0;

  isMultiple: boolean = true;
  fileProgressSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  currentFile: string = '';
  overallProgressSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  showFileProgress: boolean = false;
  showOverallProgress: boolean = false;

  @Input() id: string = '';
  @Input() multiple: string = 'false';
  @ViewChild('progressTemplate', { read: TemplateRef }) progressTemplate!:TemplateRef<any>;

  constructor(
    private _FileManagerSvc: FileManagerService,
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService
  ) { }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
    this.id = this.id.trim();
    if(!this._GWCommon.isNullOrEmpty(this.id)) {

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
    const mModalOptions: ModalOptions = new ModalOptions(this._ProgressModalId, 'Progress', this.progressTemplate, new WindowSize(150, 400));
    this._ModalSvc.open(mModalOptions);
    this._NumberOfFilesCompleted = 0;
    this._TotalNumberOfFiles = mFileList.length;
    this.fileProgressSubject.next(0);
    this.overallProgressSubject.next(0);
    this.showFileProgress = true;
    this.showOverallProgress = true;
    const mAction = this.id.replace('_Files', '');
    const mSelectedPath = this._FileManagerSvc.selectedPath;
    // Loop through all of the selected files and upload them
    for (let index = 0; index < mFileList.length; index++) {
      const mFile = mFileList[index];
      this.currentFile = mFile.name;
      this._FileManagerSvc.uploadFile(mAction, mSelectedPath, mFile);
    }
    // Clear the value so you can choose the same file(s) again
    event.target.value = '';
  }

  onProgresssDone(): void {
    this._ModalSvc.close(this._ProgressModalId);
  }
}
