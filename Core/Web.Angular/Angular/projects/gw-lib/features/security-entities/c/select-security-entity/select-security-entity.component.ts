import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
// Library
import { AccountService, IClientChoices } from '@Growthware/features/account';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
// Feature
import { SecurityEntityService } from '../../security-entity.service';
import { IValidSecurityEntity } from '../../valid-security-entity.model';

@Component({
  selector: 'gw-lib-select-security-entity',
  standalone: true,
  imports: [
    CommonModule,

    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatSelectModule
  ],
  templateUrl: './select-security-entity.component.html',
  styleUrls: ['./select-security-entity.component.scss']
})
export class SelectSecurityEntityComponent implements OnInit {

  selectedSecurityEntity: number = -1;

  validSecurityEntities: IValidSecurityEntity[] = [];

  constructor(
    private _AccountSvc: AccountService,
    private _LoggingSvc: LoggingService,
    private _SecurityEntitySvc: SecurityEntityService
  ) { }

  ngOnInit(): void {
    this._SecurityEntitySvc.getValidSecurityEntities().then((securityEntities: IValidSecurityEntity[]) => { 
      // console.log('SelectSecurityEntityComponent.ngOnInit', securityEntities);
      this.validSecurityEntities = securityEntities;
      this.selectedSecurityEntity = this._AccountSvc.clientChoices.securityEntityID;
    }).catch((error) => { 
      this._LoggingSvc.toast('Error calling the API', 'Select Security Entity', LogLevel.Error);
    });
  }

  public onSave(): void {
    // console.log('SelectSecurityEntityComponent.onSave');
    const mClientChoices: IClientChoices = JSON.parse(JSON.stringify(this._AccountSvc.clientChoices));
    mClientChoices.securityEntityID = this.selectedSecurityEntity;
    this._AccountSvc.saveClientChoices(mClientChoices).then((_) => {
      this._LoggingSvc.toast('Selection saved', 'Select Security Entity', LogLevel.Success);
    }).catch((error) => {
      this._LoggingSvc.toast('Unable to save client choices', 'Save client choices', LogLevel.Error);
    })
  }
}
