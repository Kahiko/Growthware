import { Component, OnInit } from '@angular/core';

// Interfaces / Common Code
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';

@Component({
  selector: 'gw-lib-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent implements OnInit {

  constructor(
    private _GWCommon: GWCommon,
  ) { }

  ngOnInit(): void {
  }

}
