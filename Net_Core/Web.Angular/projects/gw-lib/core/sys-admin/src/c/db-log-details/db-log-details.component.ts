import { Component } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { ModalService } from '@growthware/core/modal';
// Feature
import { SysAdminService } from '../../sys-admin.service';

@Component({
  selector: 'gw-core-db-log-details',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,

    MatButtonModule,
    MatIconModule,
    MatTabsModule
  ],
  templateUrl: './db-log-details.component.html',
  styleUrl: './db-log-details.component.scss'
})
export class DBLogDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent {

  constructor(
    private _FormBuilder: FormBuilder,
    modalSvc: ModalService,
    profileSvc: SysAdminService,
  ) {
    super();
    this._ModalSvc = modalSvc;
    this._ProfileSvc = profileSvc;
  }

  override delete(): void {
    throw new Error('Method not implemented.');
  }
  override createForm(): void {
    throw new Error('Method not implemented.');
  }
  override populateProfile(): void {
    throw new Error('Method not implemented.');
  }
  override save(): void {
    throw new Error('Method not implemented.');
  }

}
