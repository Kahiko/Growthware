import { CommonModule } from '@angular/common';
import { Component, input, OnInit } from '@angular/core';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
// Library
import { INameDataPair } from '@growthware/common/interfaces';
import { GWCommon } from '@growthware/common/services';
import { LoggingService, LogLevel } from '@growthware/core/logging';

@Component({
	selector: 'gw-core-snake-list',
	standalone: true,
	imports: [
		CommonModule,
		// Angular Material
		MatIconModule,
	],
	templateUrl: './snake-list.component.html',
	styleUrls: ['./snake-list.component.scss']
})
export class SnakeListComponent implements OnInit {

	items = input<Array<string>>([]);
	iconName = input('');
	id = input.required<string>();

	constructor(private _GWCommon: GWCommon, private _LoggingSvc: LoggingService,) { }

	ngOnInit(): void {
		this.id.apply(this.id().trim());
	}

}
