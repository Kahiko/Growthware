import { ApplicationRef, Injectable, Injector } from '@angular/core';
import { ViewContainerRef, ComponentFactoryResolver } from '@angular/core';
import { Inject, TemplateRef, Type } from '@angular/core';
import { DOCUMENT } from '@angular/common';

// Components
import { IContentObject } from './content-object.model';
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
  private _IsComponent?: boolean;
  // private _ComponentRef: any;
  private _ActiveModals: IContentObject[] = [];

  constructor(
    private _ApplicationRef: ApplicationRef,
    @Inject(DOCUMENT) private _Document: Document,
    private _GWCommon: GWCommon,
    private _Injector: Injector,
    private _LoggingSvc: LoggingService,
    // private _Resolver: ComponentFactoryResolver,
    // private _ViewContainerRef: ViewContainerRef,
  ) { }

  public close(key: string) {

  }

  public open<T>(options: IModalOptions): void {
    if(this._GWCommon.isNullOrEmpty(options.modalId)) {
      this._LoggingSvc.toast('options.modalId can not be null or blank', 'ModalService', LogLevel.Error);
      return;
    }
    const mModalComponent = this.resolveNgContent(ModalComponent);
    const mNgContent = this.resolveNgContent(options.contentPayLoad);
  }

  private resolveNgContent<T>(content: Content<T>) {
    this._IsComponent = false;
    let mRetVal: any;
    // this._ComponentRef = '';
    /** String */
    if (typeof content === 'string') {
      const element = this._Document.createTextNode(content);
      mRetVal = [[element]];
    }

    /** ngTemplate */
    if (content instanceof TemplateRef) {
      const mTemplateRef = Object.create(content) as T;
      const mViewRef = content.createEmbeddedView(mTemplateRef);
      this._ApplicationRef.attachView(mViewRef);
      mRetVal = [mViewRef.rootNodes];
    } else if (content instanceof Type) {
      /** Otherwise it's a component */
      this._IsComponent = true;
      // const mFactory = this._Resolver.resolveComponentFactory(content);
      // const mComponentRef = mFactory.create(this._Injector);
      // this._ComponentRef = mComponentRef;
      // this._ApplicationRef.attachView(mComponentRef.hostView);
      // mRetVal = [[mComponentRef.location.nativeElement]];

      //      The new way to do this?!?
      // const mComponent = this._ViewContainerRef.createComponent(content);
      // this._ApplicationRef.attachView(mComponent.hostView);
      // mRetVal = [[mComponent.location.nativeElement]];
    }
    return mRetVal;
  }
}
