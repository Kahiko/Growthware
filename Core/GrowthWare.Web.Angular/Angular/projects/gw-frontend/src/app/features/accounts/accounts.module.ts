import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library Modules
import { DynamicTableModule } from '@Growthware/Lib';
// Feature
import { AccountsRoutingModule } from './accounts-routing.module';
import { AccountProfileComponent } from './account-profile/account-profile.component';


@NgModule({
  declarations: [
    AccountProfileComponent
  ],
  imports: [
    CommonModule,
    AccountsRoutingModule
  ]
})
export class AccountsModule { }
