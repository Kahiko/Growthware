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

// Feature Components
import { MessageDetailsComponent } from './c/message-details/message-details.component';
import { SearchMessagesComponent } from './c/search-messages/search-messages.component';


@NgModule({
  declarations: [
    MessageDetailsComponent,
    SearchMessagesComponent
  ],
  imports: [
    CommonModule,

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
