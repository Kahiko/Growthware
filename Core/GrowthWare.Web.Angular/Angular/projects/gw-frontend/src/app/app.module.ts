// Third party Modules
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModules } from './material.module';
// Library Modules
import { ToastModule } from '@Growthware/Lib';
// Application Modules
import { AccountsRoutingModule } from './features/accounts/accounts-routing.module';
import { AppRoutingModule } from './app-routing.module';
import { MSDemoRoutingModule } from './ms-demo/ms-demo.routing.module';
// Application Components
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { DefaultComponent } from './skins/default/default/default.component';
import { DefaultHeaderComponent } from './skins/default/layout/default-header/default-header.component';
import { DefaultFooterComponent } from './skins/default/layout/default-footer/default-footer.component';

@NgModule({
  declarations: [
    AppComponent,
    DefaultComponent,
    DefaultHeaderComponent,
    DefaultFooterComponent,
    HomeComponent,
  ],
  imports: [
    AppRoutingModule,
    AccountsRoutingModule,
    BrowserAnimationsModule,
    BrowserModule,
    FlexLayoutModule,
    HttpClientModule,
    MaterialModules,
    MSDemoRoutingModule,
    ToastModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
