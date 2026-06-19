import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
	selector: 'gw-frontend-home',
	templateUrl: './home.component.html',
	changeDetection: ChangeDetectionStrategy.Eager,
	styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

	constructor() { }

	ngOnInit(): void {
	}

}
