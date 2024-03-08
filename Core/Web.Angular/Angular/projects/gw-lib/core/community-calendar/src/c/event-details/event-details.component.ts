import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { ModalService } from '@growthware/core/modal';
// Feature
import { CalendarService } from '../../calendar.service';
import { ICalendarEvent } from '../../interfaces/calendar-event.model';

@Component({
  selector: 'gw-core-event-details',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    // Angular Material
    MatButtonModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatTabsModule,
  ],
  templateUrl: './event-details.component.html',
  styleUrl: './event-details.component.scss'
})
export class EventDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

  private _Profile!: ICalendarEvent;

  constructor(
    profileSvc: CalendarService,
    modalSvc: ModalService,
	private _FormBuilder: FormBuilder,
  ) {
    super();
    this._ProfileSvc = profileSvc;
    this._ModalSvc = modalSvc;
    // console.log('EventDetailsComponent.selectedEvent', profileSvc.selectedEvent);
    this._Profile = profileSvc.selectedEvent;
  }

  ngOnInit(): void {
    // console.log('EventDetailsComponent.ngOnInit');
    this.createForm();
  }

  override delete(): void {
    throw new Error('Method not implemented.');
  }

  override createForm(): void {
    this.frmProfile = this._FormBuilder.group({
      title: [this._Profile.title, [Validators.required]],
    });
  }

  getErrorMessage(fieldName: string) {
    /*eslint indent: ["error", 2, { "SwitchCase": 1 }]*/
    switch (fieldName) {
      case 'title':
        if (this.controls['title'].hasError('required')) {
          return 'Required';
        }
        break;
      case 'action':
        if (this.controls['action'].hasError('required')) {
          return 'Required';
        }
        break;
      default:
        break;
    }
    return undefined;
  }

  override onSubmit(): void {
    if (this.frmProfile.valid) {
      this.populateProfile();
    }
  }

  override populateProfile(): void {
    this._Profile.title = this.controls['title'].getRawValue();
  }
  override save(): void {
    throw new Error('Method not implemented.');
  }

}
