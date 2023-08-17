import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Feature
import { LineCountComponent } from './c/line-count/line-count.component';
import { EditDbInformationComponent } from './c/edit-db-information/edit-db-information.component';

@NgModule({
  declarations: [
    LineCountComponent,
    EditDbInformationComponent
  ],
  imports: [
    CommonModule
  ]
})
export class SysAdminModule { }
