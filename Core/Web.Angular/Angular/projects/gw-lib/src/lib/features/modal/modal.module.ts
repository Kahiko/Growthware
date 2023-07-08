import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
// Feature
import { ModalComponent } from './c/popup/modal.component';
import { ModalDirectiveDirective } from './c/directive/modal-directive.directive';

// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';

@NgModule({
  declarations: [
    ModalComponent,
    ModalDirectiveDirective
  ],
  imports: [
    CommonModule,
    FormsModule,

    MatButtonModule,
    MatIconModule,
    MatToolbarModule
  ],
  exports: [
    // ModalComponent,
    ModalDirectiveDirective
  ]
})
export class ModalModule { }
