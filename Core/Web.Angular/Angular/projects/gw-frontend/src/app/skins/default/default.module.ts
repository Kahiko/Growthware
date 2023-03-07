import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
// Library Modules
import { NavigationModule } from '@Growthware/Lib';
// Modules/Components
import { DefaultRoutingModule } from './default-routing.module';
import { DefaultFooterComponent } from './default-footer/default-footer.component';
import { DefaultHeaderComponent } from './default-header/default-header.component';
import { DefaultLayoutComponent } from './default-layout/default-layout.component';


@NgModule({
  declarations: [
    DefaultFooterComponent,
    DefaultHeaderComponent,
    DefaultLayoutComponent
  ],
  imports: [
    CommonModule,
    DefaultRoutingModule,

    MatButtonModule,
    MatDividerModule,
    MatIconModule,
    MatListModule,
    MatMenuModule,
    MatSidenavModule,
    MatToolbarModule,

    NavigationModule
  ],
  exports: [
    DefaultLayoutComponent
  ]
})
export class DefaultModule { }
