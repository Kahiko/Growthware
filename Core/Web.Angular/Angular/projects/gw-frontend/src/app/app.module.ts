import { APP_INITIALIZER, NgModule } from '@angular/core';
import { UrlSerializer } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtModule } from "@auth0/angular-jwt";
// Angular Material
import { MatSelectModule } from '@angular/material/select'; // <--- Had to add b/c of an injection error when loading component from library that uses angular material
// Library Services
import { AccountService } from '@Growthware/Lib/src/lib/features/account';
import { LoaderService } from '@Growthware/Lib/src/lib/features/loader';
// Library Modules
import { LoaderModule } from '@Growthware/Lib/src/lib/features/loader';
import { ToastModule } from '@Growthware/Lib/src/lib/features/toast';
// Library Misc
import { AuthGuard } from '@Growthware/Lib/src/lib/guards/auth.guard';
import { LowerCaseUrlSerializer } from '@Growthware/Lib/src/lib/common-code';
// Application Modules
import { AppRoutingModule } from './app-routing.module';
import { DefaultModule } from './skins/default/default.module';
import { SystemModule } from './skins/system/system.module';
// Application Components
import { AppComponent } from './app.component';
// Application
import { appInitializer } from './app.initializer';
import { LoaderInterceptor } from './interceptors/loader.interceptor';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

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
    LoaderModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5001"],
        disallowedRoutes: []
      }
    }),
    MatSelectModule,
    ReactiveFormsModule,
    SystemModule,
    ToastModule,
  ],
  providers: [
    AuthGuard,
    { provide: UrlSerializer, useClass: LowerCaseUrlSerializer },
    { provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [AccountService] },
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true, deps: [LoaderService] },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
