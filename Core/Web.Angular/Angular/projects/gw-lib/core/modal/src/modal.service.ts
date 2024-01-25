import { ApplicationRef, ComponentRef, createComponent } from '@angular/core';
import { Inject, Injectable, TemplateRef, Type } from '@angular/core';
import { EmbeddedViewRef } from '@angular/core';
import { DOCUMENT } from '@angular/common';
// Library
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { GWCommon } from '@growthware/common/services';
// Features
import { ContentObject, IContentObject } from './content-object.model';
import { ContentType } from './content-type.enum';
import { ModalComponent } from './c/popup/modal.component';
import { IModalOptions } from './modal-options.model';

type Content<T> = string | TemplateRef<T> | Type<T>;

@Injectable({
	providedIn: 'root'
})
export class ModalService {
	private _ActiveModals: IContentObject[] = [];
	private _ContentType: ContentType = ContentType.String;

	constructor(
    private _ApplicationRef: ApplicationRef,
    @Inject(DOCUMENT) private _Document: Document,
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
	) { }

	/**
   * Closes a modal for the specified key.
   *
   * @param {string} key - The key of the modal to close.
   */
	public close(key: string) {
		const mContentObj = this._ActiveModals.find((obj: IContentObject) => obj.key.toUpperCase() === key.toUpperCase() as string);
		if (mContentObj !== undefined) {
			if(mContentObj.contentType === ContentType.Component) {
				try {
					// destroy child
					this._ApplicationRef.detachView(mContentObj.payloadRef.hostView);
					mContentObj.payloadRef.destroy();
				} catch (error) {
					let mMsg;
					if (error instanceof Error) {
						mMsg = error.message;
					} else {
						mMsg = String(error);
					}
					this._LoggingSvc.console(mMsg, LogLevel.Error);
				}
			}
			// remove and destroy the modal component
			this._ApplicationRef.detachView(mContentObj.modalComponentRef.hostView);
			mContentObj.modalComponentRef.destroy();
			this._ActiveModals = this._ActiveModals.filter(obj => obj !== mContentObj);
		}
	}

	/**
   * Opens a modal dialog with either string, ng-template or a component.
   *
   * @param {IModalOptions} options - The options for the modal dialog.
   * @return {void} 
   */
	public open<T>(options: IModalOptions): void {
		if(this._GWCommon.isNullOrEmpty(options.modalId)) {
			this._LoggingSvc.toast('options.modalId can not be null or blank', 'Modal Service', LogLevel.Error);
			// console.log('ModalService.open', options);
			return;
		}
		if (this._GWCommon.isNullOrEmpty(options.contentPayLoad) && typeof (options.contentPayLoad) !== 'function') {
			this._LoggingSvc.toast('Please set the contentPayLoad property', 'Modal Service', LogLevel.Error);
			return;
		}
		// resolve the ngContent
		const mResolvedNgContent = this.resolveNgContent(options.contentPayLoad);
		// first, create the child
		let mNgContent: any = mResolvedNgContent;
		if(this._ContentType === ContentType.Component) {
			// get root nodes
			mNgContent = (<EmbeddedViewRef<any>>mResolvedNgContent.hostView).rootNodes;
		}
		// then create the dialog that will host it
		const mModalComponentRef: ComponentRef<ModalComponent> = createComponent(ModalComponent, {
			environmentInjector: this._ApplicationRef.injector,
			projectableNodes: [mNgContent], // pass the child here
		});
		mModalComponentRef.instance.setUp(options); // sets up UI properties (height, width, show components, etc.)
		this.setupModalCallbacks(options, mModalComponentRef.instance);
		// append to body, we will use platform document for this
		const mDialogElement = (<EmbeddedViewRef<any>>mModalComponentRef.hostView).rootNodes[0];
		// setup a ContentObject to add to the array
		const mContentObject = new ContentObject(options.modalId, this._ContentType, mModalComponentRef);
		if(this._ContentType === ContentType.Component) { // the payloadRef is only used when it's a component so destroy can be called in the this.close
			mContentObject.payloadRef = mResolvedNgContent;
		}
		this._ActiveModals.push(mContentObject);
		// append the dialog element to the body :-)
		this._Document.body.append(mDialogElement);
		// attach the ModalComponentRef host view to the application view
		this._ApplicationRef.attachView(mModalComponentRef.hostView);
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
		this._ContentType = ContentType.String;
		let mRetVal: any;
		if (typeof content === 'string') {            /** String */
			const element = this._Document.createTextNode(content);
			mRetVal = [[element]];
		} else if (content instanceof TemplateRef) {  /** ngTemplate */
			this._ContentType = ContentType.Template;
			const mTemplateRef = Object.create(content) as T;
			const mViewRef = content.createEmbeddedView(mTemplateRef);
			this._ApplicationRef.attachView(mViewRef);
			mRetVal = [mViewRef.rootNodes];
		} else if (content instanceof Type) {         /** Otherwise it's a component */
			this._ContentType = ContentType.Component;
			mRetVal = createComponent(content, { environmentInjector: this._ApplicationRef.injector });
			// attach ComponentRef to the application reference
			this._ApplicationRef.attachView(mRetVal.hostView);

		}
		return mRetVal;
	}
  
	/**
   * Sets up the ModalComponents callback methods as defined in the modal options.
   *
   * @param {IModalOptions} modalOptions - The modal options.
   * @param {ModalComponent} mModalComponentInstance - The instace of the ModalComponent
   */
	private setupModalCallbacks(modalOptions: IModalOptions, mModalComponentInstance: ModalComponent) {
		if (this._GWCommon.isFunction(modalOptions.buttons.cancelButton.callbackMethod)) {
			mModalComponentInstance.cancelCallBackMethod = modalOptions.buttons.cancelButton.callbackMethod;
		} else {
			mModalComponentInstance.cancelCallBackMethod = () => {
				this.close(modalOptions.modalId);
			};
		}
		if (this._GWCommon.isFunction(modalOptions.buttons.closeButton.callbackMethod)) {
			mModalComponentInstance.closeCallBackMethod = modalOptions.buttons.closeButton.callbackMethod;
		} else {
			if (modalOptions.buttons.closeButton.visible) {
				this._LoggingSvc.toast('You have not set the options.buttons.closeButton.callbackMethod', 'Modal Service', LogLevel.Error);
			}
		}
		if (this._GWCommon.isFunction(modalOptions.buttons.okButton.callbackMethod)) {
			mModalComponentInstance.oKCallBackMethod = modalOptions.buttons.okButton.callbackMethod;
		} else {
			if (modalOptions.buttons.okButton.visible) {
				this._LoggingSvc.toast('You have not set the options.buttons.okButton.callbackMethod', 'Modal Service', LogLevel.Error);
			}
		}
	}

}
