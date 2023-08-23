import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
// Feature
import { LineCountComponent } from './c/line-count/line-count.component';
import { EditDbInformationComponent } from './c/edit-db-information/edit-db-information.component';

@NgModule({
  declarations: [
    LineCountComponent,
    EditDbInformationComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatInputModule
  ]
})
export class SysAdminModule { }
