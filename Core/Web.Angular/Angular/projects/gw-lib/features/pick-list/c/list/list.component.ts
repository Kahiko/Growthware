import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
import { DataNVP } from '@Growthware/shared/models';
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LogDestination, ILogOptions, LogOptions } from '@Growthware/features/logging';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
import { ModalOptions, ModalService, ModalSize } from '@Growthware/features/modal';

@Component({
  selector: 'gw-lib-list',
  standalone: true,
  imports: [
    CommonModule,

    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent {
  @Input() allItemsText: string = '';
  @Input() header: string = '';
  @Input() id: string = '';
  @Input() name: string = '';
  @Input() pickListTableHelp: string = '';
  @Input() selectedItemsText: string = '';
  @Input() size: string = '8';
  @Input() width: string = '120';

  private _AvailableItemsSubject = new BehaviorSubject<any[]>([]);
  private _AvailableItemsData: any[] = [];
  private _ModalOptions!: ModalOptions;

  readonly availableItems$ = this._AvailableItemsSubject.asObservable();

  private _Subscriptions: Subscription = new Subscription();


  constructor(
    private _DataSvc: DataService,
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService
  ) {
    // nothing atm
  }

  ngOnInit(): void {
    if (!this._GWCommon.isNullOrUndefined(this.id) && !this._GWCommon.isNullOrEmpty(this.id)) {
      this._ModalOptions = new ModalOptions(this.id + '_Modal', this.header, this.pickListTableHelp, ModalSize.Small)
      this._Subscriptions.add(
        this._DataSvc.dataChanged.subscribe((results: DataNVP) => {
          if (this.name.trim().toLowerCase() + '_availableitems' === results.name.trim().toLowerCase()) {
            // update the local data
            this._AvailableItemsData = results.payLoad;
            this._AvailableItemsSubject.next(this._AvailableItemsData);
          };
        })
      );
    } else {
      const mLogDestinations: Array<LogDestination> = [];
      mLogDestinations.push(LogDestination.Console);
      mLogDestinations.push(LogDestination.Toast);
      const mLogOptions: ILogOptions = new LogOptions(
        'PickListComponent.ngOnInit: id is blank',
        LogLevel.Error,
        mLogDestinations,
        'PickListComponent',
        'PickListComponent',
        'ngOnInit',
        'system',
        'PickListComponent'
      )
      this._LoggingSvc.log(mLogOptions);
    }
  }

  onShowHelp(): void {
    this._ModalSvc.open(this._ModalOptions);
  }
}