import { ApplicationRef, Injectable, Injector } from '@angular/core';
import { Inject, TemplateRef, Type } from '@angular/core';
import { ComponentFactoryResolver } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { Subject } from 'rxjs';

// Components
import { ContentObject, IContentObject } from './content-object.model';
import { ModalComponent } from './c/popup/modal.component';

// Services
import { LoggingService, LogLevel } from '@Growthware/Lib/src/lib/features/logging';

// Interfaces / Common Code
import { IModalOptions } from './modal-options.model';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';

type Content<T> = string | TemplateRef<T> | Type<T>;

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private _CancelCalledSubject = new Subject<string>();
  private _CloseCalledSubject = new Subject<string>();
  private _OpenCalledSubject = new Subject<IModalOptions>();

  readonly cancelCalled$ = this._CancelCalledSubject.asObservable();
  readonly closeCalled$ = this._CloseCalledSubject.asObservable();
  readonly openCalled$ = this._OpenCalledSubject.asObservable();

  constructor(
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
  ) { }

  public cancel(key: string) {
    this._CancelCalledSubject.next(key);
  }

  public close(key: string) {
    this._CloseCalledSubject.next(key);
  }

  public open<T>(options: IModalOptions): void {
    if(this._GWCommon.isNullOrEmpty(options.modalId)) {
      this._LoggingSvc.toast('options.modalId can not be null or blank', 'Modal Service', LogLevel.Error);
      return;
    }
    if (this._GWCommon.isNullOrEmpty(options.contentPayLoad) && typeof (options.contentPayLoad) !== 'function') {
      this._LoggingSvc.toast('Please set the contentPayLoad property', 'Modal Service', LogLevel.Error);
      return;
    }
    this._OpenCalledSubject.next(options);
  }
}
