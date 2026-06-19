
import { Component, input, OnInit, ChangeDetectionStrategy } from '@angular/core';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
// Library
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';

@Component({
    selector: 'gw-core-snake-list',
    imports: [
    MatIconModule
],
    templateUrl: './snake-list.component.html',
    changeDetection: ChangeDetectionStrategy.Eager,
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
