import { ApplicationRef, Inject, createComponent, Directive, EnvironmentInjector , OnInit } from '@angular/core';
import { OnDestroy, TemplateRef, Type } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { Subscription } from 'rxjs';
// Feature
import { ModalComponent } from '../popup/modal.component';
import { ModalService } from '../../modal.service';
import { IModalOptions } from '../../modal-options.model';
import { ContentObject, IContentObject } from '../../content-object.model';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService, LogLevel } from '@Growthware/features/logging';

type Content<T> = string | TemplateRef<T> | Type<T>;

@Directive({
  selector: '[gwModalDirective]'
})
export class ModalDirectiveDirective implements OnDestroy, OnInit {

  private _ActiveModals: IContentObject[] = [];
  // private _ComponentRef: any;
  private _IsComponent: boolean = false;
  private _Subscription: Subscription = new Subscription();

  constructor(
    private _ApplicationRef: ApplicationRef,
    @Inject(DOCUMENT) private _Document: Document,
    private _EnvironmentInjector: EnvironmentInjector,
    private _GWCommon: GWCommon,
    // private _Injector: Injector,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    // private _ViewContainerRef: ViewContainerRef
  ) { }

  ngOnDestroy(): void {
    this._Subscription.unsubscribe();
  }

  /**
   * Initializes the component and subscribes to openCalled$, cancelCalled$, and closeCalled$ observables.
   *
   * @param {} - No parameters.
   * @return {} - No return value.
   */
  ngOnInit() {
    this._Subscription.add(
      this._ModalSvc.openCalled$.subscribe((modalOptions: IModalOptions) => {
        const mKey = modalOptions.modalId;
        // Create an instance of the ContentObject
        const mContentObject = new ContentObject(mKey, this._IsComponent, null, null);
        // Resolve the ngContent
        const mResolvedContent = this.resolveNgContent(modalOptions.contentPayLoad);
        let mNgContent = mResolvedContent;
        // If the ngContent is a Component, Set payloadRef
        if(this._IsComponent) {
          mResolvedContent.changeDetectorRef.detectChanges();
          mContentObject.payloadRef = mResolvedContent;
          mContentObject.isComponent = true;
          mNgContent = [[mResolvedContent.location.nativeElement]];
        }
        // Get a reference to the ModalComponent
        const mModalComponentRef = createComponent(ModalComponent, { environmentInjector: this._EnvironmentInjector, projectableNodes: [mNgContent] });
        // Create an instance of the ModalComponent
        const mModalComponent = mModalComponentRef.instance as ModalComponent;
        mContentObject.nativeElement = mModalComponentRef.location.nativeElement;
        // Send the modalOptions to the ModalComponent.setUp
        mModalComponent.setUp(modalOptions);
        this.setUpCallBacks(modalOptions, mModalComponent);
        mContentObject.modalComponentRef = mModalComponentRef;
        this._GWCommon.baseURL;
        // Add the modal to the ui
        document.body.appendChild(mContentObject.nativeElement);
        // Register the newly created ModalComponent ref using the `ApplicationRef` instance
        this._ApplicationRef.attachView(mContentObject.modalComponentRef.hostView);
        // Include the component view into change detection cycles 
        mContentObject.modalComponentRef.changeDetectorRef.detectChanges();
        // Add the ContentObject to the _ActiveModals
        this._ActiveModals.push(mContentObject);
      })
    );

    this._Subscription.add(this._ModalSvc.cancelCalled$.subscribe((key: string) => {
      this.cancel(key);  
    }));

    this._Subscription.add(this._ModalSvc.closeCalled$.subscribe((key: string) => {
      this.close(key);  
    }));
  }

  setUpCallBacks(modalOptions: IModalOptions, modalComponent: ModalComponent) {
    if (this._GWCommon.isFunction(modalOptions.buttons.cancelButton.callbackMethod)) {
      modalComponent.cancelCallBackMethod = modalOptions.buttons.cancelButton.callbackMethod;
    } else {
      modalComponent.cancelCallBackMethod = () => {
        this.close(modalOptions.modalId);
      };
    }
    if (this._GWCommon.isFunction(modalOptions.buttons.closeButton.callbackMethod)) {
      modalComponent.closeCallBackMethod = modalOptions.buttons.closeButton.callbackMethod;
    } else {
      if(modalOptions.buttons.closeButton.visible) {
        this._LoggingSvc.toast('You have not set the options.buttons.closeButton.callbackMethod', 'Modal Service', LogLevel.Error)
      }
    }
    if (this._GWCommon.isFunction(modalOptions.buttons.okButton.callbackMethod)) {
      modalComponent.oKCallBackMethod = modalOptions.buttons.okButton.callbackMethod;
    } else {
      if(modalOptions.buttons.okButton.visible) {
        this._LoggingSvc.toast('You have not set the options.buttons.okButton.callbackMethod', 'Modal Service', LogLevel.Error)
      }
    }    
  }

  /**
   * Calls close passing the key.
   *
   * @param {string} key - The key of the task to be cancelled.
   */
  private cancel(key: string) {
    this.close(key);
  }

  /**
   * Closes a modal based on the provided key.
   *
   * @private
   * @param {string} key
   * @memberof ModalDirectiveDirective
   */
  private close(key: string) {
    const mContentObj = this._ActiveModals.find((obj: IContentObject) => obj.key.toUpperCase() === key.toUpperCase() as string);
    if (mContentObj !== undefined) {
      try {
        this._ActiveModals = this._ActiveModals.filter(obj => obj !== mContentObj);
        this._Document.body.removeChild(mContentObj.nativeElement);
        if(mContentObj.isComponent) {
          mContentObj.payloadRef.destroy();
        }
        mContentObj.modalComponentRef.destroy();
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

  /**
   * Resolves the ngContent of a given Content object.
   *
   * @template T - The type of the content.
   * @param {Content<T>} content - The content to resolve.
   * @return {any} - The resolved ngContent.
   * @memberof ModalDirectiveDirective
   */  
  private resolveNgContent<T>(content: Content<T>) {
    this._IsComponent = false;
    let mRetVal: any;
    if (typeof content === 'string') {            /** String */
      const element = this._Document.createTextNode(content);
      mRetVal = element;
    } else if (content instanceof TemplateRef) {  /** ngTemplate */
      const mTemplateRef = Object.create(content) as T;
      const mViewRef = content.createEmbeddedView(mTemplateRef);
      mRetVal = mViewRef.rootNodes;
    } else if (content instanceof Type) {         /** Otherwise it's a component */
      this._IsComponent = true;
      // const mComponentRef = this._ViewContainerRef.createComponent<any>(content, {injector: this._Injector});
      // this._ComponentRef = this._ViewContainerRef.createComponent<any>(content, {injector: this._Injector});
      // mRetVal = [[this._ViewContainerRef.createComponent<any>(content, {injector: this._Injector}).location.nativeElement]];
      mRetVal = createComponent(content, { environmentInjector: this._EnvironmentInjector});
    }
    return mRetVal;
  }
}
