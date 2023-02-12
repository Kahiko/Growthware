import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { TemplateRef, ViewChild } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
// Library
import { ModalOptions, ModalService, WindowSize } from '@Growthware/Lib/src/lib/features/modal';
// Feature

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

  constructor(private _ModalSvc: ModalService) { }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
  }

  onFileSelected(event: any): void {  
    const mFileList: FileList = {...event.target.files};
    const mModalOptions: ModalOptions = new ModalOptions(this._ProgressModalId, 'Progress', this.progressTemplate, new WindowSize(150, 400));
    this._ModalSvc.open(mModalOptions);
    this._NumberOfFilesCompleted = 0;
    this._TotalNumberOfFiles = mFileList.length;
    this.fileProgressSubject.next(0);
    this.overallProgressSubject.next(0);
    this.showFileProgress = true;
    this.showOverallProgress = true;    
    // Clear the value so you can choose the same file(s) again
    event.target.value = '';
  }

  onProgresssDone(): void {
    this._ModalSvc.close(this._ProgressModalId);
  }
}
