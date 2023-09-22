import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSelectModule } from '@angular/material/select';
// Library
import { DynamicTableModule } from '@Growthware/features/dynamic-table';
// Feature Components
import { SearchMessagesComponent } from './c/search-messages/search-messages.component';

@NgModule({
  declarations: [
    SearchMessagesComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,

    MatButtonModule,
    MatCheckboxModule,
    MatFormFieldModule, 
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatTabsModule,
    MatSelectModule
  ]
})
export class MessageModule { }
