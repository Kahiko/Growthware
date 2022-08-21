import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

// Feature
import { AccountsRoutingModule } from './accounts-routing.module';

@NgModule({
  declarations: [
    // SearchAccountsComponent,
  ],
  imports: [
    AccountsRoutingModule,
    CommonModule,
    MatButtonModule,
  ]
})
export class AccountsModule { }
