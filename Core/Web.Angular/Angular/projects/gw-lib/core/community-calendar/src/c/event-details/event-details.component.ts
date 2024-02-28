import { Component, OnInit } from '@angular/core';
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';

@Component({
	selector: 'gw-core-event-details',
	standalone: true,
	imports: [],
	templateUrl: './event-details.component.html',
	styleUrl: './event-details.component.scss'
})
export class EventDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

	constructor() {
		super();
	}

	ngOnInit(): void {
		console.log('EventDetailsComponent.ngOnInit');
	}

	override delete(): void {
		throw new Error('Method not implemented.');
	}
	override createForm(): void {
		throw new Error('Method not implemented.');
	}
	override populateProfile(): void {
		throw new Error('Method not implemented.');
	}
	override save(): void {
		throw new Error('Method not implemented.');
	}

}
