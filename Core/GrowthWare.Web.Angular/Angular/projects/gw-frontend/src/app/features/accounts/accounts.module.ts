import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// Feature
import { AccountsRoutingModule } from './accounts-routing.module';

@NgModule({
  declarations: [
    // SearchAccountsComponent,
  ],
  imports: [
    CommonModule,
    AccountsRoutingModule,
  ]
})
export class AccountsModule { }
