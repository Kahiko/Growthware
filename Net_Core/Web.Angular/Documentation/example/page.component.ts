import { Component, inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
// Angular Material
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { IModalOptions, ModalOptions, ModalService, WindowSize } from '@growthware/core/modal';
import { ILogOptions, LogDestination, LoggingService, LogOptions } from '@growthware/core/logging';
// Feature

@Component({
  selector: 'gw-core-test-logging',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    // Angular Material
    MatIconModule,
    MatTabsModule,
  ],
  templateUrl: './page.compontent.html',
  styleUrl: './page.component.scss'
})
export class TestLoggingComponent implements OnInit {

  private _FormBuilder = inject(FormBuilder);
  private _LoggingSvc = inject(LoggingService);

  theForm: FormGroup = this._FormBuilder.group({});

  ngOnInit(): void {
    this.createForm();
  }

  createForm(): void {
    const mLogOptions: ILogOptions = new LogOptions(
        this.controls['msg'].getRawValue(),
        this.controls['selectedLogLevel'].getRawValue(),
        [LogDestination.Toast],
        this.controls['componentName'].getRawValue(),
        this.controls['className'].getRawValue(),
        this.controls['methodName'].getRawValue(),
        this.controls['account'].getRawValue(),
        this.controls['title'].getRawValue()
      );
    this._LoggingSvc.log(mLogOptions);
    this.theForm = this._FormBuilder.group({

    });
  }

  get controls() {
    return this.theForm.controls;
  }

  onSubmit(form: FormGroup): void {
    console.log('TestLoggingComponent.onSubmit: form ', form);
  }
}
