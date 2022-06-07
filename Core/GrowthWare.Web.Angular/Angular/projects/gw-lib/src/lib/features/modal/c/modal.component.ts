import { Component, OnInit } from '@angular/core';
import { ISecurityInfo, SecurityInfo } from '@Growthware/Lib/src/lib/models';

@Component({
  selector: 'gw-lib-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent implements OnInit {
  public clientMessage: string = '';
  public securityInfo: ISecurityInfo = new SecurityInfo();
  constructor() { }

  ngOnInit(): void {
  }

}
