import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { provideNativeDateAdapter } from '@angular/material/core';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { LogLevel } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
// Feature
import { CalendarService } from '../../calendar.service';
import { ICalendarEvent } from '../../interfaces/calendar-event.model';
import { TimePickerComponent } from '../time-picker/time-picker.component';
import { INameValuePair, NameValuePair } from '@growthware/common/interfaces';

@Component({
  selector: 'gw-core-event-details',
  standalone: true,
  providers: [
    provideNativeDateAdapter(),
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    // Angular Material
    MatButtonModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatTabsModule,
    // Feature
    TimePickerComponent,
  ],
  templateUrl: './event-details.component.html',
  styleUrl: './event-details.component.scss'
})
export class EventDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

  private _Action: string = '';
  private _Profile!: ICalendarEvent;
  endDate!: Date;
  readonly selectedEventDate = new FormControl(new Date());
  selectedColor: string = 'Blue';
  startDate!: Date;
  validColors: INameValuePair[] = [];


  constructor(
    profileSvc: CalendarService,
    modalSvc: ModalService,
    private _FormBuilder: FormBuilder,
    private _Router: Router,
  ) {
    super();
    this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
    this._ProfileSvc = profileSvc;
    this._ModalSvc = modalSvc;
    // console.log('EventDetailsComponent.selectedEvent', profileSvc.selectedEvent);
    this._Profile = profileSvc.selectedEvent;
    this.createForm();
    if (this._Profile.id > 0) {
      this.canDelete = true;
    }
  }

  ngOnInit(): void {
    // console.log('EventDetailsComponent.ngOnInit');
    this.validColors.push(new NameValuePair('#6495ED', 'Blue'));
    this.validColors.push(new NameValuePair('#ff0000', 'Red'));
    if (this._Profile.id > -1) {
      this._ProfileSvc.getEventSecurity(this._Profile.id).then((canEdit: boolean) => {
        this.canDelete = canEdit;
        this.canSave = canEdit;
      });
    } else {
      this.canSave = true;
    }
  }

  override delete(): void {
    this._ProfileSvc.deleteEvent(this._Profile.id, this._Action).then(() => {
      this.onClose();
    }).catch(() => {
      console.log('EventDetailsComponent.delete');
      this._LoggingSvc.toast('The event could not be deleted', 'Delete Group', LogLevel.Error);
    });
  }

  override createForm(): void {
    this.selectedEventDate.setValue(new Date(this._Profile.start));
    this.selectedColor = this._Profile.color;
    this.endDate = new Date(this._Profile.end);
    this.startDate = new Date(this._Profile.start);
    this.frmProfile = this._FormBuilder.group({
      allDay: [this._Profile.allDay],
      description: [this._Profile.description],
      endDate: [this._Profile.end, [Validators.required]],
      link: [this._Profile.link],
      location: [this._Profile.location],
      startDate: [this._Profile.start, [Validators.required]],
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
      // console.log('EventDetailsComponent.onSubmit.Updated Profile', this._Profile);
      this.save();
    }
  }

  override populateProfile(): void {
    if (this.selectedEventDate.value) {
      const mNewDate = new Date(this.selectedEventDate.value);
      this.startDate.setFullYear(mNewDate.getFullYear(), mNewDate.getMonth(), mNewDate.getDate());
    }
    this._Profile.allDay = this.controls['allDay'].getRawValue();
    this._Profile.color = this.selectedColor;
    this._Profile.description = this.controls['description'].getRawValue();
    this._Profile.end = this.endDate.toString();
    this._Profile.link = this.controls['link'].getRawValue();
    this._Profile.location = this.controls['location'].getRawValue();
    this._Profile.start = this.startDate.toString();
    this._Profile.title = this.controls['title'].getRawValue();
  }

  override save(): void {
    this._ProfileSvc.saveEvent(this._Action, this._Profile).then(() => {
      this.onClose();
    });
  }

  timeRangeSelected(event: { startDate: Date, endDate: Date }) {
    this.endDate = event.endDate;
    this.startDate = event.startDate;
  }

}
