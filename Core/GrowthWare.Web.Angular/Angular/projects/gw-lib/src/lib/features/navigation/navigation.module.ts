import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VerticalListItemComponent } from './c/vertical-list-item/vertical-list-item.component';

import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';

@NgModule({
  declarations: [
    VerticalListItemComponent
  ],
  imports: [
    CommonModule,
    MatIconModule,
    MatListModule
  ],
  exports: [
    VerticalListItemComponent
  ]
})
export class NavigationModule { }
