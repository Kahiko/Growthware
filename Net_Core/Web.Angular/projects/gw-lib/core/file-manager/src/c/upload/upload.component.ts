import { Component, computed, effect, input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
// Library
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { ModalOptions, ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { FileManagerService } from '../../file-manager.service';

@Component({
	selector: 'gw-core-upload',
	standalone: true,
	imports: [
		CommonModule,
		// Angular Material
		MatButtonModule,
		MatIconModule,
		MatProgressBarModule,
	],
	templateUrl: './upload.component.html',
	styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {
	private _Action: string = '';
	private _NumberOfFilesCompleted: number = 0;
	private _ProgressModalId: string = 'progressTemplate';
	private _TotalNumberOfFiles: number = 0;

	id: string = '';
	isMultiple: boolean = true;
	fileProgress: number = 0;
	currentFile: string = '';
	overallProgress: number = 0;
	showFileProgress: boolean = false;
	showOverallProgress: boolean = false;

	multiple = input<string>('false');
	@ViewChild('progressTemplate', { read: TemplateRef }) private _ProgressTemplate!: TemplateRef<unknown>;

	constructor(
		private _FileManagerSvc: FileManagerService,
		private _GWCommon: GWCommon,
		private _LoggingSvc: LoggingService,
		private _ModalSvc: ModalService,
		private _Router: Router
	) {
		effect(() => {
			const mData = computed(() => this._FileManagerSvc.uploadStatusChanged$());
			if (mData().id.toLowerCase() + '_upload' === this.id.toLowerCase()) {
				const mFilePercentage: number = Math.floor((mData().uploadNumber / mData().totalNumberOfUploads) * 100);
				this.fileProgress = mFilePercentage;
				if (mFilePercentage == 100) {
					this._NumberOfFilesCompleted = this._NumberOfFilesCompleted + 1;
					const mTotalPercent: number = Math.floor((this._NumberOfFilesCompleted / this._TotalNumberOfFiles) * 100);
					this.overallProgress = mTotalPercent;
					if (mTotalPercent == 100) {
						this._GWCommon.sleep(500).then(() => {
							this.showFileProgress = false;
							// this._FileManagerSvc.refresh(this._Action);
							this._GWCommon.sleep(3000).then(() => {
								this.onOk();
							});
						});
					}
				}
			}
		});
	}

	ngOnInit(): void {
		this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
		this.id = this._Action + '_Upload';
	}

	onFileSelected(event: Event): void {
		const mFleInput = event.target as HTMLInputElement;
		const mFileList: FileList | null = mFleInput.files;
		if (mFileList) {
			const mModalOptions: ModalOptions = new ModalOptions(this._ProgressModalId, 'Progress', this._ProgressTemplate, new WindowSize(150, 400));
			this._ModalSvc.open(mModalOptions);
			this._NumberOfFilesCompleted = 0;
			this._TotalNumberOfFiles = mFileList.length;
			this.fileProgress = 0;
			this.overallProgress = 0;
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
			mFleInput.value = '';
		}
	}

	onOk(): void {
		this._ModalSvc.close(this._ProgressModalId);
	}
}
