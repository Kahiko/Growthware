import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormGroup } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
// Library
import { LoggingService, LogLevel } from '@Growthware/features/logging';
import { IKeyValuePair } from '@Growthware/shared/models';
// Feature


@Component({
  selector: 'gw-lib-copy-function-security',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
    ReactiveFormsModule,

    MatButtonModule,
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

  validSecurityEntities: IKeyValuePair[] = [];

  constructor(
    private _FormBuilder: FormBuilder,
    private _LoggingSvc: LoggingService,
  ) { 
    this.createForm();
  }

  ngOnInit(): void {
    this._LoggingSvc.toast('Save not working yet', 'Copy Function:', LogLevel.Error);
  }

  createForm(): void {
    this.frmProfile = this._FormBuilder.group({

    });
  }

  onSubmit(form: FormGroup): void {
    // this._LoggingSvc.toast('BaseDetailComponent.onSubmit', 'Function Details:', LogLevel.Error);
    this.populateProfile();
    this.save();
  }

  private populateProfile(): void {
    
  }

  private save(): void {
    
  }

}
