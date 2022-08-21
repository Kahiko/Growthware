import { Component, Input, OnInit } from '@angular/core';
import { ModalOptions, ModalService } from '@Growthware/Lib/src/lib/features/modal';

@Component({
  selector: 'gw-lib-pick-list',
  templateUrl: './pick-list.component.html',
  styleUrls: ['./pick-list.component.scss']
})
export class PickListComponent implements OnInit {
  private _ModalOptions!: ModalOptions;
  sortOnChange: boolean = true;

  @Input() allItemsText: string = '';
  @Input() header: string = '';
  @Input() id: string = '';
  @Input() name: string = '';
  @Input() pickListTableHelp: string = '';
  @Input() selectedItemsText: string = '';
  @Input() size: string = '8';
  @Input() width: string = '120';

  constructor(private _ModalSvc: ModalService) { }

  ngOnInit(): void {
    this._ModalOptions = new ModalOptions(this.id + '_Modal', this.header, this.pickListTableHelp)
  }

  onMove(listBox: string, direction: string): void {

  }

  onShowHelp(): void {
    this._ModalSvc.open(this._ModalOptions);
  }

}
