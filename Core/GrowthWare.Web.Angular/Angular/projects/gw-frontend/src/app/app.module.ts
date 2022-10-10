import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from "@auth0/angular-jwt";
// Library Modules
import { ToastModule } from '@Growthware/Lib/src/lib/features/toast';
// Application Modules
import { AppRoutingModule } from './app-routing.module';
import { DefaultModule } from './skins/default/default.module';
import { SystemModule } from './skins/system/system.module';
// Application Components
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    DefaultModule,
    HttpClientModule,
    JwtModule,
    SystemModule,
    ToastModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
