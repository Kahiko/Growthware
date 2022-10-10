import { NgModule } from '@angular/core';
import { UrlSerializer } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from "@auth0/angular-jwt";
// Library Modules
import { ToastModule } from '@Growthware/Lib/src/lib/features/toast';
import { LowerCaseUrlSerializer } from '@Growthware/Lib/src/lib/common-code';
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
    FormsModule,
    HttpClientModule,
    JwtModule,
    ReactiveFormsModule,
    SystemModule,
    ToastModule,
  ],
  providers: [  {
    provide: UrlSerializer,
    useClass: LowerCaseUrlSerializer
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
