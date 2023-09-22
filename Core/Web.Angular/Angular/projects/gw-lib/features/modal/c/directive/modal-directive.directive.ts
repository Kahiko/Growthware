import { Inject, Directive, OnInit, ViewContainerRef, Injector } from '@angular/core';
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
export class ModalDirective implements OnDestroy, OnInit {

  private _ActiveModals: IContentObject[] = [];
  private _IsComponent: boolean = false;
  private _Subscription: Subscription = new Subscription();

  constructor(
    @Inject(DOCUMENT) private _Document: Document,
    private _Injector: Injector,
    private _LoggingSvc: LoggingService,
    private _ModalSvc: ModalService,
    private _GWCommon: GWCommon,
    private _ViewContainerRef: ViewContainerRef
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
        const mNgContent = this.resolveNgContent(modalOptions.contentPayLoad);
        const mComponentRef = this._ViewContainerRef.createComponent<any>(ModalComponent, {
          projectableNodes: [mNgContent]
        });
        const mModalComponent = (mComponentRef.instance as ModalComponent);

        // setup the callback methods here
        if (this._GWCommon.isFunction(modalOptions.buttons.cancelButton.callbackMethod)) {
          mModalComponent.cancelCallBackMethod = modalOptions.buttons.cancelButton.callbackMethod;
        } else {
          mModalComponent.cancelCallBackMethod = () => {
            this.close(modalOptions.modalId);
          };
        }
        if (this._GWCommon.isFunction(modalOptions.buttons.closeButton.callbackMethod)) {
          mModalComponent.closeCallBackMethod = modalOptions.buttons.closeButton.callbackMethod;
        } else {
          if(modalOptions.buttons.closeButton.visible) {
            this._LoggingSvc.toast('You have not set the options.buttons.closeButton.callbackMethod', 'Modal Service', LogLevel.Error)
          }
        }
        if (this._GWCommon.isFunction(modalOptions.buttons.okButton.callbackMethod)) {
          mModalComponent.oKCallBackMethod = modalOptions.buttons.okButton.callbackMethod;
        } else {
          if(modalOptions.buttons.okButton.visible) {
            this._LoggingSvc.toast('You have not set the options.buttons.okButton.callbackMethod', 'Modal Service', LogLevel.Error)
          }
        }
        // let the modal component work with any changes made to it
        mComponentRef.hostView.detectChanges();
        // setup the callback methods here
        if (this._GWCommon.isFunction(modalOptions.buttons.cancelButton.callbackMethod)) {
          mModalComponent.cancelCallBackMethod = modalOptions.buttons.cancelButton.callbackMethod;
        } else {
          mModalComponent.cancelCallBackMethod = () => {
            this.close(modalOptions.modalId);
          };
        }
        if (this._GWCommon.isFunction(modalOptions.buttons.closeButton.callbackMethod)) {
          mModalComponent.closeCallBackMethod = modalOptions.buttons.closeButton.callbackMethod;
        } else {
          if(modalOptions.buttons.closeButton.visible) {
            this._LoggingSvc.toast('You have not set the options.buttons.closeButton.callbackMethod', 'Modal Service', LogLevel.Error)
          }
        }
        if (this._GWCommon.isFunction(modalOptions.buttons.okButton.callbackMethod)) {
          mModalComponent.oKCallBackMethod = modalOptions.buttons.okButton.callbackMethod;
        } else {
          if(modalOptions.buttons.okButton.visible) {
            this._LoggingSvc.toast('You have not set the options.buttons.okButton.callbackMethod', 'Modal Service', LogLevel.Error)
          }
        }
        // send the options to finish setting up the modal component's properties
        mModalComponent.setUp(modalOptions);
        const { nativeElement } = mComponentRef.location;
        const mContentObject = new ContentObject(modalOptions.modalId, this._IsComponent, mComponentRef, nativeElement)
        this._ActiveModals.push(mContentObject);
        this._Document.body.appendChild(nativeElement);
      })
    );

    this._Subscription.add(this._ModalSvc.cancelCalled$.subscribe((key: string) => {
      this.cancel(key);  
    }));

    this._Subscription.add(this._ModalSvc.closeCalled$.subscribe((key: string) => {
      this.close(key);  
    }));
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
   * @memberof ModalDirective
   */
  private close(key: string) {
    const mContentObj = this._ActiveModals.find((obj: IContentObject) => obj.key.toUpperCase() === key.toUpperCase() as string);
    if (mContentObj !== undefined) {
      try {
        this._ActiveModals = this._ActiveModals.filter(obj => obj !== mContentObj);
        this._Document.body.removeChild(mContentObj.nativeElement);
        if(mContentObj.isComponent) {
          mContentObj.modalComponentRef.destroy();
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

  /**
   * Resolves the ngContent of a given Content object.
   *
   * @template T - The type of the content.
   * @param {Content<T>} content - The content to resolve.
   * @return {any} - The resolved ngContent.
   * @memberof ModalDirective
   */  
  private resolveNgContent<T>(content: Content<T>) {
    this._IsComponent = false;
    let mRetVal: any;
    if (typeof content === 'string') {            /** String */
      const element = this._Document.createTextNode(content);
      mRetVal = [[element]];
    } else if (content instanceof TemplateRef) {  /** ngTemplate */
      const mTemplateRef = Object.create(content) as T;
      const mViewRef = content.createEmbeddedView(mTemplateRef);
      mRetVal = [mViewRef.rootNodes];
    } else if (content instanceof Type) {         /** Otherwise it's a component */
      this._IsComponent = true;
      const mComponentRef = this._ViewContainerRef.createComponent<any>(content, {injector: this._Injector});
      mRetVal = [[mComponentRef.location.nativeElement]];
    }
    return mRetVal;
  }
}
