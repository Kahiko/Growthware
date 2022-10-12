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
import { AccountsRoutingModule } from './features/accounts/accounts-routing.module';
import { AppRoutingModule } from './app-routing.module';
import { DefaultModule } from './skins/default/default.module';
import { FunctionsRoutingModule } from './features/functions/functions-routing.module';
import { SystemModule } from './skins/system/system.module';
// Application Components
import { AppComponent } from './app.component';
// Application
import { AuthGuard } from './guards/auth.guard';

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AppRoutingModule,
    AccountsRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    DefaultModule,
    FormsModule,
    FunctionsRoutingModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5001"],
        disallowedRoutes: []
      }
    }),
    ReactiveFormsModule,
    SystemModule,
    ToastModule,
  ],
  providers: [
    AuthGuard, {
    provide: UrlSerializer,
    useClass: LowerCaseUrlSerializer
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
