import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTreeModule } from '@angular/material/tree';
import { ScrollingModule } from '@angular/cdk/scrolling';
// Library
import { DynamicTableModule } from '@Growthware/src/features/dynamic-table';
import { SortByModule } from '@Growthware/src/features/sort-by';
import { SnakeListModule } from '@Growthware/src/features/snake-list';
// Feature
import { FileManagerComponent } from './c/file-manager/file-manager.component';
import { UploadComponent } from './c/upload/upload.component';
import { FileListComponent } from './c/file-list/file-list.component';
import { DirectoryTreeComponent } from './c/directory-tree/directory-tree.component';

@NgModule({
  declarations: [
    FileManagerComponent,
    UploadComponent,
    FileListComponent,
    DirectoryTreeComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatProgressBarModule,
    MatToolbarModule,
    MatTreeModule,
    ReactiveFormsModule,
    ScrollingModule,
    SortByModule,
    SnakeListModule
  ]
})
export class FileManagerModule { }
