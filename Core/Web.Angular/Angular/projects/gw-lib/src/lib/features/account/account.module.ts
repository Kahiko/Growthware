import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
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
import { DynamicTableModule } from '@Growthware/Lib/src/lib/features/dynamic-table';
import { PickListModule } from '@Growthware/Lib/src/lib/features/pick-list';
import { SnakeListModule } from '@Growthware/Lib/src/lib/features/snake-list';
// Feature Components
import { AccountDetailsComponent } from './c/account-details/account-details.component';
import { ChangePasswordComponent } from './c/change-password/change-password.component';
import { SearchAccountsComponent } from './c/search-accounts/search-accounts.component';
import { LoginComponent } from './c/login/login.component';
import { LogoutComponent } from './c/logout/logout.component';
// Feature Modules
import { AccountsRoutingModule } from './accounts-routing.module';
import { SelectPreferencesComponent } from './c/select-preferences/select-preferences.component';
import { UpdateAnonymousProfileComponent } from './c/update-anonymous-profile/update-anonymous-profile.component';


@NgModule({
  declarations: [
    AccountDetailsComponent,
    ChangePasswordComponent,
    SearchAccountsComponent,
    LoginComponent,
    LogoutComponent,
    SelectPreferencesComponent,
    UpdateAnonymousProfileComponent,
  ],
  imports: [
    AccountsRoutingModule,
    CommonModule,
    DynamicTableModule,
    FormsModule,
    MatButtonModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatTabsModule,
    MatSelectModule,
    PickListModule,
    ReactiveFormsModule,
    SnakeListModule,
  ]
})
export class AccountModule { }
