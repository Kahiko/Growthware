import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTreeModule } from '@angular/material/tree';
import { ScrollingModule } from '@angular/cdk/scrolling';
// Library
import { SortByModule } from '@Growthware/Lib/src/lib/features/sort-by';
import { SnakeListModule } from '@Growthware/Lib/src/lib/features/snake-list';
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
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatProgressBarModule,
    MatToolbarModule,
    MatTreeModule,
    ScrollingModule,
    SortByModule,
    SnakeListModule
  ]
})
export class FileManagerModule { }
