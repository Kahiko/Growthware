import { CommonModule } from '@angular/common';
import { Component, inject, input, OnInit, signal } from '@angular/core';
import { TemplateRef, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
// Library
import { ModalOptions, ModalService, WindowSize } from '@growthware/core/modal';
// Feature
import { AddDirectoryComponent } from '../add-directory/add-directory.component';
import { DirectoryTreeComponent } from '../directory-tree/directory-tree.component';
import { FileListComponent } from '../file-list/file-list.component';
import { FileManagerService } from '../../file-manager.service';
import { TableFileListComponent } from '../table-file-list/table-file-list.component';
import { UploadComponent } from '../upload/upload.component';

@Component({
	selector: 'gw-core-file-manager',
	standalone: true,
	imports: [
		CommonModule,
		// Angular Material
		MatButtonModule,
		MatIconModule,
		MatMenuModule,
		MatToolbarModule,
		// Library
		DirectoryTreeComponent,
		TableFileListComponent,
		FileListComponent,
		UploadComponent
	],
	templateUrl: './file-manager.component.html',
	styleUrls: ['./file-manager.component.scss']
})
export class FileManagerComponent implements OnInit {
    private _FileManagerSvc = inject(FileManagerService);
    private _ModalSvc = inject(ModalService);
    private _Router = inject(Router);

	private _Action: string = '';
	private _ModalId_CreateDirectory: string = 'CreateDirectoryForm';

	fileContent: string = 'snake';
	frmCreateDirectory!: FormGroup;
	validFileContents = [
		{id: 'snake', name: 'List', icon: 'summarize'},
		// {id: 'details', name: 'Details', icon: 'list'},
		{id: 'table', name: 'Table', icon: 'table_rows'},
	];

	skin$ = signal<string>('default');

  configurationName = input<string>();

  @ViewChild('helpText', { read: TemplateRef }) private _HelpText!:TemplateRef<unknown>;

  ngOnInit(): void {
  	const mAction: string = this._Router.url.split('?')[0] .replace('/', '').replace('\\','');
  	this._Action = mAction;
  	this.configurationName.apply(mAction);
  	this._FileManagerSvc.getDirectories(mAction, '\\');
  }

  onCreateDirectory() {
  	// console.log('item', item);
  	const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_CreateDirectory, 'New Folder', AddDirectoryComponent, new WindowSize(84, 300));
  	this._ModalSvc.open(mModalOptions);
  }

  onHelp(): void {
  	const mModalOptions: ModalOptions = new ModalOptions(this._ModalId_CreateDirectory, 'Help', this._HelpText, new WindowSize(325, 550));
  	this._ModalSvc.open(mModalOptions);
  }

  onRefresh(): void {
  	this._FileManagerSvc.refresh(this._Action);
  }

  setLayout(layout: string): void {
	this.skin$.update(() =>layout);
  }
}
