import { Component, OnInit } from '@angular/core';
// Feature
import { SecurityService } from '../../security.service';
import { LoggingService } from '@Growthware/Lib/src/features/logging';

@Component({
  selector: 'gw-lib-guid-helper',
  templateUrl: './guid-helper.component.html',
  styleUrls: ['./guid-helper.component.scss']
})
export class GuidHelperComponent implements OnInit {

  guidText: string = '';

  constructor(
    private _LoggingSvc: LoggingService,
    private _SecuritySvc: SecurityService
  ) { }

  ngOnInit(): void {
  }

  onGuid(): void {
    this._SecuritySvc.getGuid().catch((error: any)=>{
      this._LoggingSvc.errorHandler(error, 'GuidHelperComponent', 'onGuid');
    }).then((response)=>{
      if(response) {
        this.guidText = response;
      }
    });
  }

}
