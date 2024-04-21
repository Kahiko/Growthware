import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// This Feature
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './c/home/home.component';
import { GenericHomeComponent } from './c/generic-home/generic-home.component';

@NgModule({
	declarations: [
		GenericHomeComponent,
		HomeComponent
	],
	imports: [
		CommonModule,
		HomeRoutingModule
	]
})
export class HomeModule { }
