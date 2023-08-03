import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
import { DataNVP } from '@Growthware/Lib/src/lib/models';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';

@Component({
  selector: 'gw-lib-snake-list',
  templateUrl: './snake-list.component.html',
  styleUrls: ['./snake-list.component.scss']
})
export class SnakeListComponent implements OnDestroy, OnInit {

  private _DataSubject = new BehaviorSubject<any[]>([]);
  private _Subscriptions: Subscription = new Subscription();

  readonly data$ = this._DataSubject.asObservable();
  @Input() id: string = '';

  constructor(private _DataSvc: DataService, private _GWCommon: GWCommon, private _LoggingSvc: LoggingService,) { }

  ngOnDestroy(): void {
    this._Subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.id = this.id.trim();
    if(this._GWCommon.isNullOrUndefined(this.id)) {
      this._LoggingSvc.toast('The is can not be blank!', 'Snake List Component', LogLevel.Error);
    } else {
      this._Subscriptions.add(this._DataSvc.dataChanged.subscribe((data: DataNVP) => {
        if(data.name.toLocaleLowerCase() === this.id.toLowerCase()) {
          this._DataSubject.next(data.payLoad);
        }
      }));
    }
  }

}
