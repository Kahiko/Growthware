import { Component, Input, OnInit } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
import { DataNVP } from '@Growthware/Lib/src/lib/models';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';

@Component({
  selector: 'gw-lib-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.scss']
})
export class FileListComponent implements OnInit {

  private _DataSubject = new BehaviorSubject<any[]>([]);
  private _Subscriptions: Subscription = new Subscription();

  readonly data = this._DataSubject.asObservable();

  @Input() id: string = '';
  @Input() numberOfColumns: string = '4';

  constructor(private _DataSvc: DataService, private _GWCommon: GWCommon, private _LoggingSvc: LoggingService,) { }

  ngOnDestroy(): void {
    this._Subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.id = this.id.trim();
    if(this._GWCommon.isNullOrUndefined(this.id)) {
      this._LoggingSvc.toast('The is can not be blank!', 'File List Component', LogLevel.Error);
    } else {
      this._Subscriptions.add(this._DataSvc.dataChanged.subscribe((data: DataNVP) => {
        if(data.name.toLocaleLowerCase() === this.id.toLowerCase()) {
          this._DataSubject.next(data.payLoad);
        }
      }));
    }
  }

  getTemplateColumnsStyle(): Object {
    let obj :Object;
    obj = { 'grid-template-columns':'repeat('+Number(this.numberOfColumns)+', 1fr)'};
    // console.log('obj', obj);
    return obj;
  }

}

