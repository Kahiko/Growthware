import { ApplicationRef, Injectable, ComponentFactoryResolver, Injector } from '@angular/core';
import { Inject, TemplateRef, Type } from '@angular/core';
import { DOCUMENT } from '@angular/common';

// Components
import { IContentObject } from './content-object.model';
import { ModalComponent } from './c/modal.component';

// Interfaces / Common Code
import { IModalOptions } from './modal-options.model';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';

type Content<T> = string | TemplateRef<T> | Type<T>;

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private _IsComponent: boolean;
  // private _ComponentRef: any;
  private _ActiveModals: IContentObject[] = [];

  constructor(
    private _ApplicationRef: ApplicationRef,
    @Inject(DOCUMENT) private _Document: Document,
    private _GWCommon: GWCommon,
    private _Injector: Injector,
    private _Resolver: ComponentFactoryResolver,
  ) { }

  public close(key: string) {

  }

  public open<T>(options: IModalOptions): void {

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
      const mFactory = this._Resolver.resolveComponentFactory(content);
      const mComponentRef = mFactory.create(this._Injector);
      // this._ComponentRef = mComponentRef;
      this._ApplicationRef.attachView(mComponentRef.hostView);
      mRetVal = [[mComponentRef.location.nativeElement]];
    }
    return mRetVal;
  }
}
