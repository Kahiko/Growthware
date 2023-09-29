import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
// Feature
import { SecurityEntityService } from '../../security-entity.service';

@Component({
  selector: 'gw-lib-select-secutiry-entity',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './select-secutiry-entity.component.html',
  styleUrls: ['./select-secutiry-entity.component.scss']
})
export class SelectSecutiryEntityComponent {

  constructor(
    private _securityEntitySvc: SecurityEntityService
  ) { 
    this._securityEntitySvc.getValidSecurityEntities().then((response: any) => {
      console.log(response);
    })
  }

}
