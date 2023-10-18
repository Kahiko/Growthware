import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library
import { DynamicTableModule } from '@Growthware/features/dynamic-table';
// Feature
import { SearchSecurityEntitiesComponent } from './c/search-security-entities/search-security-entities.component';

@NgModule({
  declarations: [
    SearchSecurityEntitiesComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
  ]
})
export class SecurityEntitiesModule { }
