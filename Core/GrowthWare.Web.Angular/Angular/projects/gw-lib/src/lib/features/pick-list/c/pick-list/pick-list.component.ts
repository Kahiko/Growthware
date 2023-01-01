import { AfterViewInit, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subject, Subscription } from 'rxjs';
// Library
import { DataNVP } from '@Growthware/Lib/src/lib/models';
import { DataService } from '@Growthware/Lib/src/lib/services';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';
import { ModalOptions, ModalService } from '@Growthware/Lib/src/lib/features/modal';

@Component({
  selector: 'gw-lib-pick-list',
  templateUrl: './pick-list.component.html',
  styleUrls: ['./pick-list.component.scss']
})
export class PickListComponent implements AfterViewInit, OnDestroy, OnInit {
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
  private _Subscriptions: Subscription = new Subscription();
  private _SelectedItemsSubject = new BehaviorSubject<any[]>([]);
  private _SelectedItemsData: any[] = [];

  readonly availableItems = this._AvailableItemsSubject.asObservable();
  readonly selectedItems = this._SelectedItemsSubject.asObservable();
  sortOnChange: boolean = true;

  constructor(private _DataSvc: DataService, private _GWCommon: GWCommon, private _ModalSvc: ModalService) { }

  ngOnDestroy(): void {
    this._Subscriptions.unsubscribe();
  }

  ngAfterViewInit(): void {

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
        if (this.name.trim().toLowerCase() + '_selecteditems' === results.name.trim().toLowerCase()) {
          // update the local data
          this._SelectedItemsData = results.payLoad;
          this._SelectedItemsSubject.next(results.payLoad);
          for (let mOutterIndex = 0; mOutterIndex < this._SelectedItemsData.length; mOutterIndex++) {
            for (let mInnerIndex = 0; mInnerIndex < this._AvailableItemsData.length; mInnerIndex++) {
              if (this._SelectedItemsData[mOutterIndex] == this._AvailableItemsData[mInnerIndex]) {
                const mIndexInArray = this._AvailableItemsData.indexOf(this._SelectedItemsData[mOutterIndex], 0);
                this._AvailableItemsData.splice(mIndexInArray, 1);
                break;
              }
            }
          }
          let sortedArray = this._AvailableItemsData.slice();
          sortedArray.sort(function (a, b) {
            return GWCommon.naturalSort(a, b);
          });
          this._AvailableItemsSubject.next(sortedArray);
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
