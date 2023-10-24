import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormGroup } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService, LogLevel } from '@Growthware/features/logging';
import { IKeyValuePair, KeyValuePair } from '@Growthware/shared/models';
import { SecurityEntityService } from '@Growthware/features/security-entities';
// Feature


@Component({
  selector: 'gw-lib-copy-function-security',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
    ReactiveFormsModule,

    MatButtonModule,
    MatFormFieldModule,
    MatIconModule, 
    MatInputModule,
    MatSelectModule,
  ],
  templateUrl: './copy-function-security.component.html',
  styleUrls: ['./copy-function-security.component.scss']
})
export class CopyFunctionSecurityComponent implements OnInit {

  frmProfile!: FormGroup;
  selectedSource: number = -1;
  selectedTarget: number = -1;

  validSourceEntities: IKeyValuePair[] = [];
  validTargetEntities: IKeyValuePair[] = [];

  constructor(
    private _FormBuilder: FormBuilder,
    private _LoggingSvc: LoggingService,
    private _SecuritySvc: SecurityEntityService
  ) { 
    this.createForm();
  }

  ngOnInit(): void {
    this._SecuritySvc.getValidParents(1).then((securityEntities) => {
      this.validSourceEntities = JSON.parse(JSON.stringify(securityEntities));
      this.validTargetEntities = JSON.parse(JSON.stringify(securityEntities));
      const mSystem: IKeyValuePair = new KeyValuePair();
      mSystem.key = 1;
      mSystem.value = 'System';
      this.validSourceEntities.push(mSystem);
      this.validSourceEntities = GWCommon.sortArray(this.validSourceEntities, 'value', 'desc');
    }).catch((error: any) => {
      this._LoggingSvc.errorHandler(error, 'CopyFunctionSecurityComponent', 'ngOnInit');
    });
  }

  createForm(): void {
    this.frmProfile = this._FormBuilder.group({});
  }

  onSubmit(form: FormGroup): void {
    this._LoggingSvc.toast('Save is not working yet', 'Copy Function:', LogLevel.Error);
  }
}
