import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library
import { DynamicTableModule } from '@Growthware/features/dynamic-table';
// Feature
import { SecurityEntityDetailsComponent } from './c/security-entity-details/security-entity-details.component';
import { SearchSecurityEntitiesComponent } from './c/search-security-entities/search-security-entities.component';

@NgModule({
  declarations: [
    SecurityEntityDetailsComponent,
    SearchSecurityEntitiesComponent
  ],
  imports: [
    CommonModule,
    DynamicTableModule,
  ]
})
export class SecurityEntitiesModule { }