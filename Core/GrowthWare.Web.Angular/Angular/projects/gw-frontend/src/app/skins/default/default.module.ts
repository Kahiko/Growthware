import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
// Modules/Components
import { DefaultRoutingModule } from './default-routing.module';
import { DefaultComponent } from './default/default.component';
import { DefaultFooterComponent } from './layout/default-footer/default-footer.component';
import { DefaultHeaderComponent } from './layout/default-header/default-header.component';
import { DefaultSidebarComponent } from './layout/default-sidebar/default-sidebar.component';

@NgModule({
  declarations: [
    DefaultComponent,
    DefaultFooterComponent,
    DefaultHeaderComponent,
    DefaultSidebarComponent
  ],
  imports: [
    CommonModule,
    DefaultRoutingModule,
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
