// Third party Modules
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModules } from './material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from "@auth0/angular-jwt";
// Library Modules
import { ToastModule } from '@Growthware/Lib';
import { NavigationModule } from '@Growthware/Lib';
// Application Modules
import { AuthGuard } from './guards/auth.guard';
import { AccountsRoutingModule } from './features/accounts/accounts-routing.module';
import { AppRoutingModule } from './app-routing.module';
import { MSDemoRoutingModule } from './ms-demo/ms-demo.routing.module';
// Application Components
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { DefaultComponent } from './skins/default/default-layout/default.component';
import { DefaultHeaderComponent } from './skins/default/default-header/default-header.component';
import { DefaultFooterComponent } from './skins/default/default-footer/default-footer.component';

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

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
    FormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5001"],
        disallowedRoutes: []
      }
    }),
    MaterialModules,
    MSDemoRoutingModule,
    NavigationModule,
    ReactiveFormsModule,
    ToastModule,
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent],
})
export class AppModule {}
