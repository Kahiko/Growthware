import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
// Feature
import { SetLogLevelComponent } from './c/set-log-level/set-log-level.component';

@NgModule({
  declarations: [
    SetLogLevelComponent
  ],
  imports: [
    CommonModule,
    FormsModule,

    MatButtonModule,
    MatSelectModule,
  ]
})
export class LoggingModule { }
