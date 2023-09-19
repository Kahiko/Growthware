import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { PickListModule } from '@Growthware/features/pick-list';

@Component({
  selector: 'gw-lib-role-details',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    PickListModule,
    ReactiveFormsModule,

    MatButtonModule,
    MatCheckboxModule,
    MatIconModule, 
    MatInputModule,
    MatTabsModule,
  ],
  templateUrl: './role-details.component.html',
  styleUrls: ['./role-details.component.scss']
})
export class RoleDetailsComponent implements OnDestroy, OnInit {

  accountPickListName: string = 'accountsList';
  canDelete: boolean = false;
  frmRole!: FormGroup;

  constructor(
    private _FormBuilder: FormBuilder,
  ) { }

  ngOnInit(): void {
    this.populateForm();
  }

  ngOnDestroy(): void {
    // nothing atm
  }

  get controls() {
    return this.frmRole.controls;
  }

  getErrorMessage(fieldName: string) {
    switch (fieldName) {
      case 'name':
        if (this.controls['name'].hasError('required')) {
          return 'Required';
        }
        break;
      default:
        break;
    }
    return undefined;
  }

  onCancel(): void {
    // nothing atm
  }

  onDelete(): void {
    // nothing atm
  }

  onSubmit(form: FormGroup): void {
    // nothing atm
  }

  private populateForm(): void {
    this.frmRole = this._FormBuilder.group({
      name: ['', Validators.required],
      description: [''],
      isSystem :[{value : false, disabled: !true}],
      isSystemOnly :[{value : false, disabled: !true}],
    });
  }
}
