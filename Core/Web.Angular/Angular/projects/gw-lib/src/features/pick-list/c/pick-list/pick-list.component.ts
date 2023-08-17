import { AfterViewInit, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
// Library
import { DataNVP } from '@Growthware/src/shared/models';
import { DataService } from '@Growthware/src/shared/services';
import { GWCommon } from '@Growthware/src/common-code';
import { LogDestination, ILogOptions, LogOptions } from '@Growthware/src/features/logging';
import { LoggingService, LogLevel } from '@Growthware/src/features/logging';
import { ModalOptions, ModalService } from '@Growthware/src/features/modal';
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

  readonly availableItems$ = this._AvailableItemsSubject.asObservable();
  readonly selectedItems$ = this._SelectedItemsSubject.asObservable();
  sortOnChange: boolean = true;

  constructor(
    private _DataSvc: DataService,
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService) { }

  ngOnDestroy(): void {
    this._Subscriptions.unsubscribe();
  }

  ngAfterViewInit(): void {

  }

  ngOnInit(): void {
    if (!this._GWCommon.isNullOrUndefined(this.id) && !this._GWCommon.isNullOrEmpty(this.id)) {
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
            for (let mOuterIndex = 0; mOuterIndex < this._SelectedItemsData.length; mOuterIndex++) {
              for (let mInnerIndex = 0; mInnerIndex < this._AvailableItemsData.length; mInnerIndex++) {
                if (this._SelectedItemsData[mOuterIndex] == this._AvailableItemsData[mInnerIndex]) {
                  const mIndexInArray = this._AvailableItemsData.indexOf(this._SelectedItemsData[mOuterIndex], 0);
                  this._AvailableItemsData.splice(mIndexInArray, 1);
                  break;
                }
              }
            }
            this.sortDataArrays();
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

  onMove(listBox: string, direction: string): void {

  }

  onShowHelp(): void {
    this._ModalSvc.open(this._ModalOptions);
  }

  switchAll(e: any, source: string, destination: string): void {
    e.stopPropagation();
    e.preventDefault();
    var objFromBox = document.getElementById(this.id + source)! as HTMLSelectElement;
    // var objToBox = document.getElementById(this.id + destination)! as HTMLSelectElement;
    if(objFromBox.length > 0) {
      if(source == '_SrcList') {
        // remove all from _AvailableItemsData add to _SelectedItemsData
        for (let mOuterIndex = 0; mOuterIndex < objFromBox.length; mOuterIndex++) {
          this._SelectedItemsData.push(objFromBox.item(mOuterIndex)?.text);
        }
        this._AvailableItemsData = [];
        this.sortDataArrays();
      } else {
        // remove all from _SelectedItemsData add to _AvailableItemsData
        for (let mOuterIndex = 0; mOuterIndex < objFromBox.length; mOuterIndex++) {
          this._AvailableItemsData.push(objFromBox.item(mOuterIndex)?.text);
        }
        this._SelectedItemsData = [];
        this.sortDataArrays();
      }
    }
  }

  switchList(e: any, source: string, destination: string): void {
    e.stopPropagation();
    e.preventDefault();
    var objFromBox = document.getElementById(this.id + source)! as HTMLSelectElement;
    // var objToBox = document.getElementById(this.id + destination)! as HTMLSelectElement;
    if(objFromBox.selectedOptions.length > 0) {
      if(source == '_SrcList') {
        // remove from _AvailableItemsData add to _SelectedItemsData
        for (let mOuterIndex = 0; mOuterIndex < objFromBox.selectedOptions.length; mOuterIndex++) {
          for (let mInnerIndex = 0; mInnerIndex < this._AvailableItemsData.length; mInnerIndex++) {
            if(objFromBox.selectedOptions[mOuterIndex].text == this._AvailableItemsData[mInnerIndex]) {
              this._AvailableItemsData.splice(mInnerIndex, 1);
              break;
            }
          }
          this._SelectedItemsData.push(objFromBox.selectedOptions[mOuterIndex].text);
        }
        this.sortDataArrays();
      } else {
        // remove from _SelectedItemsData add to _AvailableItemsData
        for (let mOuterIndex = 0; mOuterIndex < objFromBox.selectedOptions.length; mOuterIndex++) {
          for (let mInnerIndex = 0; mInnerIndex < this._SelectedItemsData.length; mInnerIndex++) {
            if(objFromBox.selectedOptions[mOuterIndex].text == this._SelectedItemsData[mInnerIndex]) {
              this._SelectedItemsData.splice(mInnerIndex, 1);
              break;
            }
          }
          this._AvailableItemsData.push(objFromBox.selectedOptions[mOuterIndex].text);
        }
        this.sortDataArrays();
      }
    }
  }

  private sortDataArrays(): void{
    let sortedArray = this._AvailableItemsData.slice();
    sortedArray.sort(function (a, b) {
      return GWCommon.naturalSort(a, b);
    });
    this._AvailableItemsSubject.next(sortedArray);

    sortedArray = this._SelectedItemsData.slice();
    sortedArray.sort(function (a, b) {
      return GWCommon.naturalSort(a, b);
    });
    this._SelectedItemsSubject.next(sortedArray);
    this._DataSvc.notifyDataChanged(this.id, sortedArray);
  }

}
