import { Component, inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { TestLoggingComponent } from '../test-logging/test-logging.component';
import { ModalService, ModalOptions, ModalSize, WindowSize } from '@growthware/core/modal';

@Component({
  selector: 'gw-core-test-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    // Angular Material
    MatButtonModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatRadioModule,
    MatSelectModule,
    MatTabsModule,
  ],
  templateUrl: './test-modal.component.html',
  styleUrl: './test-modal.component.scss'
})
export class TestModalComponent implements OnInit {

  private _FormBuilder = inject(FormBuilder);
  private _ModalService = inject(ModalService);
  private _ModalOptions = new ModalOptions('testComponent', 'Component', '', 1);
  private _ModalOptionsOne = new ModalOptions('First', 'Component', '', 1);
  private _ModalOptionsTwo = new ModalOptions('Second', 'Component', '', 1);

  @ViewChild('templateRef', { read: TemplateRef }) private _TemplateRef!: TemplateRef<unknown>;
  theForm: FormGroup = this._FormBuilder.group({});


  ngOnInit(): void {
    this.createForm();
  }

  createForm(): void {
    this.theForm = this._FormBuilder.group({
      msg: ['Just a message to test with', Validators.required],
      selectedPayload: ['string', Validators.required],
      title: ['My Title', Validators.required],
    });
  }

  get controls() {
    return this.theForm.controls;
  }

  onSubmit(): void {
    const mPayload: string = this.controls['selectedPayload'].getRawValue();
    switch (mPayload) {
      case 'component':
        /** $height: '575px'; $width: '575px';   */
        this._ModalOptions.windowSize = new WindowSize(585, 575);
        this._ModalOptions.headerText = 'Component - ';
        this._ModalOptions.contentPayLoad = TestLoggingComponent;
        break;
      case 'string':
        this._ModalOptions.windowSize = ModalSize.Small;
        this._ModalOptions.headerText = 'String - ';
        this._ModalOptions.contentPayLoad = this.controls['msg'].getRawValue();
        break;
      case 'templateRef':
        this._ModalOptions.headerText = 'templateRef - ';
        this._ModalOptions.contentPayLoad = this._TemplateRef;
        this._ModalOptions.windowSize = ModalSize.Normal;
        break;
      case 'double':
        this._ModalOptionsOne.headerText = 'String - ';
        this._ModalOptionsOne.contentPayLoad = 'First Modal';
        this._ModalOptionsTwo.headerText = 'String - ';
        this._ModalOptionsTwo.contentPayLoad = 'Second Modal';
        this._ModalService.open(this._ModalOptionsOne);
        this._ModalService.open(this._ModalOptionsTwo);
        return;
    }
    this._ModalOptions.headerText = this._ModalOptions.headerText + this.controls['title'].getRawValue();
    this._ModalService.open(this._ModalOptions);
  }
}
