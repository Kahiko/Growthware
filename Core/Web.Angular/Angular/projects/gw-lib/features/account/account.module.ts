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
import { MatRadioModule } from '@angular/material/radio';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSelectModule } from '@angular/material/select';
// Library
import { DynamicTableModule } from '@Growthware/features/dynamic-table';
import { PickListModule } from '@Growthware/features/pick-list';
import { SnakeListModule } from '@Growthware/features/snake-list';
// Feature Components
import { AccountDetailsComponent } from './c/account-details/account-details.component';
import { ChangePasswordComponent } from './c/change-password/change-password.component';
import { SearchAccountsComponent } from './c/search-accounts/search-accounts.component';
import { LoginComponent } from './c/login/login.component';
import { LogoutComponent } from './c/logout/logout.component';
// Feature Modules
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
    CommonModule,
    DynamicTableModule,
    FormsModule,

    MatButtonModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatRadioModule,
    MatSelectModule,
    MatTabsModule,

    PickListModule,
    ReactiveFormsModule,
    SnakeListModule,
  ]
})
export class AccountModule { }