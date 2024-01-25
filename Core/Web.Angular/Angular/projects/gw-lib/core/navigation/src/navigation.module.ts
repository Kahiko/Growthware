import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
// Feature
import { VerticalListItemComponent } from './c/vertical-list-item/vertical-list-item.component';

@NgModule({
	declarations: [
		VerticalListItemComponent
	],
	imports: [
		CommonModule,
		MatIconModule,
		MatListModule,
		RouterModule
	],
	exports: [
		VerticalListItemComponent
	]
})
export class NavigationModule { }
