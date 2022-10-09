import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
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
import { DefaultComponent } from './default-layout/default.component';
import { DefaultFooterComponent } from './default-footer/default-footer.component';
import { DefaultHeaderComponent } from './default-header/default-header.component';

@NgModule({
  declarations: [
    DefaultComponent,
    DefaultFooterComponent,
    DefaultHeaderComponent,
  ],
  imports: [
    CommonModule,
    DefaultRoutingModule,
    FlexLayoutModule,
    NavigationModule,
    MatButtonModule,
    MatDividerModule,
    MatIconModule,
    MatListModule,
    MatMenuModule,
    MatSidenavModule,
    MatToolbarModule,
  ],
  exports:[
    DefaultComponent
  ]
})
export class DefaultModule { }
