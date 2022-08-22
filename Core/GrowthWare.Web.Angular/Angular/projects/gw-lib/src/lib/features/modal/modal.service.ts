import { ApplicationRef, Injectable, Injector } from '@angular/core';
import { Inject, TemplateRef, Type } from '@angular/core';
import { ComponentFactoryResolver } from '@angular/core';
import { DOCUMENT } from '@angular/common';

// Components
import { ContentObject, IContentObject } from './content-object.model';
import { ModalComponent } from './c/modal.component';

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
  // https://github.com/fullstackio/awesome-fullstack-tutorials/blob/master/angular/dynamic-components-with-content-projection/dynamic-components-projection.md
  private _IsComponent: boolean = false;
  private _ComponentRef: any;
  private _ActiveModals: IContentObject[] = [];

  private _CancelCallBackMethod!: (arg?: any) => void;
  private _CloseCallBackMethod!: (arg?: any) => void;
  private _OKCallBackMethod!: (arg?: any) => void;

  constructor(
    private _ApplicationRef: ApplicationRef,
    @Inject(DOCUMENT) private _Document: Document,
    private _GWCommon: GWCommon,
    private _Injector: Injector,
    private _LoggingSvc: LoggingService,
    private _Resolver: ComponentFactoryResolver,
  ) { }

  private getModalComponentRef(options: IModalOptions): any {
    const mFactory = this._Resolver.resolveComponentFactory(ModalComponent);
    const mNgContent = this.resolveNgContent(options.contentPayLoad);
    const mRetVal = mFactory.create(this._Injector, mNgContent);
    return mRetVal;
  }

  public cancel(key: string) {
    if (this._GWCommon.isFunction(this._CancelCallBackMethod)) {
      this._CancelCallBackMethod();
    } else {
      this.close(key);
    }
  }

  public close(key: string) {
    // will "close" the modal by removing the element from the document body
    const mContentObj = this._ActiveModals.find((obj: IContentObject) => obj.key.toUpperCase() === key.toUpperCase() as string);
    if (mContentObj !== undefined) {
      try {
        this._ActiveModals = this._ActiveModals.filter(obj => obj !== mContentObj);
        this._Document.body.removeChild(mContentObj.value);
        if(mContentObj.isComponent) {
          mContentObj.componentRef.destroy();
        }
      } catch (error) {
        let mMsg
        if (error instanceof Error) {
          mMsg = error.message
        } else {
          mMsg = String(error)
        }
        this._LoggingSvc.console(mMsg, LogLevel.Error);
      }
    }
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

    const mComponentRef = this.getModalComponentRef(options);
    const mModalComponent = (mComponentRef.instance as ModalComponent);
    // setup the callback methods here
    if (this._GWCommon.isFunction(options.buttons.cancelButton.callbackMethod)) {
      mModalComponent.cancelCallBackMethod = options.buttons.cancelButton.callbackMethod;
    } else {
      mModalComponent.cancelCallBackMethod = () => {
        this.close(options.modalId);
      };
    }
    if (this._GWCommon.isFunction(options.buttons.closeButton.callbackMethod)) {
      mModalComponent.closeCallBackMethod = options.buttons.closeButton.callbackMethod;
    } else {
      if(options.buttons.closeButton.visible) {
        this._LoggingSvc.toast('You have not set the options.buttons.closeButton.callbackMethod', 'Modal Service', LogLevel.Error)
      }
    }
    if (this._GWCommon.isFunction(options.buttons.okButton.callbackMethod)) {
      mModalComponent.oKCallBackMethod = options.buttons.okButton.callbackMethod;
    } else {
      if(options.buttons.okButton.visible) {
        this._LoggingSvc.toast('You have not set the options.buttons.okButton.callbackMethod', 'Modal Service', LogLevel.Error)
      }
    }
    // send the options to finish setting up the modal component's properties
    mModalComponent.setUp(options);

    mComponentRef.hostView.detectChanges(); // let the modal component work with any changes made to it
    const { nativeElement } = mComponentRef.location;
    const mContentObject = new ContentObject(this._ComponentRef, this._IsComponent, options.modalId, nativeElement)
    this._ActiveModals.push(mContentObject);
    this._Document.body.appendChild(nativeElement);
  }

  private resolveNgContent<T>(content: Content<T>) {
    this._IsComponent = false;
    let mRetVal: any;
    if (typeof content === 'string') {            /** String */
      const element = this._Document.createTextNode(content);
      mRetVal = [[element]];
    } else if (content instanceof TemplateRef) {  /** ngTemplate */
      const mTemplateRef = Object.create(content) as T;
      const mViewRef = content.createEmbeddedView(mTemplateRef);
      this._ApplicationRef.attachView(mViewRef);
      mRetVal = [mViewRef.rootNodes];
    } else if (content instanceof Type) {         /** Otherwise it's a component */
      this._IsComponent = true;
      const mFactory = this._Resolver.resolveComponentFactory(content);
      const mComponentRef = mFactory.create(this._Injector);
      this._ComponentRef = mComponentRef;
      this._ApplicationRef.attachView(mComponentRef.hostView);
      mRetVal = [[mComponentRef.location.nativeElement]];
    }
    return mRetVal;
  }
}
