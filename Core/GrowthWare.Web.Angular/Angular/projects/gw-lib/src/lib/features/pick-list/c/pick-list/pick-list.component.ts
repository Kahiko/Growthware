import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subject, Subscription } from 'rxjs';
// Library
import { DataNVP } from '@Growthware/Lib/src/lib/models';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { ModalOptions, ModalService } from '@Growthware/Lib/src/lib/features/modal';

@Component({
  selector: 'gw-lib-pick-list',
  templateUrl: './pick-list.component.html',
  styleUrls: ['./pick-list.component.scss']
})
export class PickListComponent implements OnDestroy, OnInit {
  @Input() allItemsText: string = '';
  @Input() header: string = '';
  @Input() id: string = '';
  @Input() name: string = '';
  @Input() pickListTableHelp: string = '';
  @Input() selectedItemsText: string = '';
  @Input() size: string = '8';
  @Input() width: string = '120';

  private _ModalOptions!: ModalOptions;
  private _Subscriptions: Subscription = new Subscription();
  private _AvailableItemsSubject = new BehaviorSubject<any[]>([]);
  private _AvailableItemsData: any[] = [];

  // readonly availableItems: Array<string> = [];
  readonly availableItems = this._AvailableItemsSubject.asObservable();
  sortOnChange: boolean = true;

  constructor(private _DataSvc: DataService, private _ModalSvc: ModalService) { }

  ngOnDestroy(): void {
    this._Subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this._ModalOptions = new ModalOptions(this.id + '_Modal', this.header, this.pickListTableHelp)
    this._Subscriptions.add(
      this._DataSvc.dataChanged.subscribe((results: DataNVP) => {
        if (this.name.trim().toLowerCase() + '_availableitems' === results.name.trim().toLowerCase()) {
            // update the local data
            this._AvailableItemsData = results.payLoad;
            this._AvailableItemsSubject.next(results.payLoad);
        };
      })
    );    
  }

  onMove(listBox: string, direction: string): void {

  }

  onShowHelp(): void {
    this._ModalSvc.open(this._ModalOptions);
  }

}
