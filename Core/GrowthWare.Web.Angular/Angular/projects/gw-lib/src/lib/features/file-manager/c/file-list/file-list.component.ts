import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
import { MatMenuTrigger } from '@angular/material/menu';
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
  // we create an object that contains coordinates 
  menuTopLeftPosition =  {x: '0', y: '0'} 

  @Input() id: string = '';
  @Input() numberOfColumns: string = '4';

  // reference to the MatMenuTrigger in the DOM 
  @ViewChild( MatMenuTrigger, {static: true}) matMenuTrigger!: MatMenuTrigger; 

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

  onPropertiesClick(item: any) {
    const mm: string = '';
  }

  /**
   * Method called when the user click with the right button
   * @param event MouseEvent, it contains the coordinates
   * @param item Our data contained in the row of the table
   */
  onRightClick(event: MouseEvent, item: any) {
    // preventDefault avoids to show the visualization of the right-click menu of the browser
    event.preventDefault();

    // we record the mouse position in our object
    this.menuTopLeftPosition.x = event.clientX.toString();
    this.menuTopLeftPosition.y = event.clientY.toString();

    // we open the menu
    // we pass to the menu the information about our object
    this.matMenuTrigger.menuData = {item: item}

    // we open the menu
    this.matMenuTrigger.openMenu();
  }
}

