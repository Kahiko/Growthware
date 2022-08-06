import { NgModule } from '@angular/core';
// Third party Modules
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModules } from './material.module';
// Library Modules
import { ToastModule } from '@Growthware/Lib';
// Application Modules/Components
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

import { AccountsRoutingModule } from './features/accounts/accounts-routing.module';
import { AccountsModule } from './features/accounts/accounts.module';
import { DefaultModule } from './skins/default/default.module';

@NgModule({
  declarations: [
    AppComponent,
    CounterComponent,
    // DefaultComponent,
    FetchDataComponent,
    HomeComponent,
    NavMenuComponent,
  ],
  imports: [
    AppRoutingModule,
    AccountsModule,
    AccountsRoutingModule,
    BrowserAnimationsModule,
    BrowserModule,
    DefaultModule,
    FlexLayoutModule,
    HttpClientModule,
    MaterialModules,
    ToastModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
