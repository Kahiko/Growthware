// Angular
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
// Thrid party UI
import { AngularMaterialModules } from 'src/app/app.angular.material.module';   // single line to import all Angular Material modules
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
// Application Services/Modules
import { AccountService } from './services/account.service';
import { AppRoutingModule } from './app-routing.module';
import { ConfigurationService } from 'src/app/services/configuration.service';
import { LogService } from 'src/app/services/logservice.service';
// Application UI
import { NavMenuComponent } from 'src/app/layout/nav-menu/nav-menu.component';
// Application Components
import { AppComponent } from 'src/app/layout/app.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    AngularMaterialModules,
  ],
  providers: [
    AccountService,
    ConfigurationService,
    LogService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
