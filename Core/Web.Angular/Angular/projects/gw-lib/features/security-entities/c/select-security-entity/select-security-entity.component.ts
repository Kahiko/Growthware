import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
// Library
import { LoggingService, LogLevel } from '@Growthware/features/logging';
// Feature
import { SecurityEntityService } from '../../security-entity.service';

@Component({
  selector: 'gw-lib-select-security-entity',
  standalone: true,
  imports: [
    CommonModule,
    SecurityEntityService
  ],
  templateUrl: './select-security-entity.component.html',
  styleUrls: ['./select-security-entity.component.scss']
})
export class SelectSecurityEntityComponent implements OnInit {

  constructor(
    private _LoggingSvc: LoggingService,
    private _SecurityEntitySvc: SecurityEntityService
  ) { }

  ngOnInit(): void {
    this._SecurityEntitySvc.getValidSecurityEntities().then((response) => { 
      console.log(response);
    }).catch((error: any) => { 
      this._LoggingSvc.toast('Error calling the API', 'Select Security Entity', LogLevel.Error);
    });
  }
}
