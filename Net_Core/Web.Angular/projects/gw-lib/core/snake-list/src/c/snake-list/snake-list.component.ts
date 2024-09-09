import { CommonModule } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
// Library
import { INameDataPair } from '@growthware/common/interfaces';
import { DataService, GWCommon } from '@growthware/common/services';
import { LoggingService, LogLevel } from '@growthware/core/logging';

@Component({
	selector: 'gw-core-snake-list',
	standalone: true,
	imports: [
		CommonModule,

		MatIconModule,
	],
	templateUrl: './snake-list.component.html',
	styleUrls: ['./snake-list.component.scss']
})
export class SnakeListComponent implements OnDestroy, OnInit {

	private _DataSubject = new BehaviorSubject<INameDataPair[]>([]);
	private _Subscriptions: Subscription = new Subscription();

	readonly data$ = this._DataSubject.asObservable();
	@Input() iconName: string = '';

	@Input() id: string = '';

	constructor(private _DataSvc: DataService, private _GWCommon: GWCommon, private _LoggingSvc: LoggingService,) { }

	ngOnDestroy(): void {
		this._Subscriptions.unsubscribe();
	}

	ngOnInit(): void {
		this.id = this.id.trim();
		if (this._GWCommon.isNullOrUndefined(this.id)) {
			this._LoggingSvc.toast('The is can not be blank!', 'Snake List Component', LogLevel.Error);
		} else {
			this._Subscriptions.add(this._DataSvc.dataChanged$.subscribe((data: INameDataPair) => {
				if (data.name.toLocaleLowerCase() === this.id.toLowerCase()) {
					this._DataSubject.next(data.value);
				}
			}));
		}
	}

}
