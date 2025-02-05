import { ApplicationRef, ComponentRef, createComponent } from '@angular/core';
import { Inject, Injectable, TemplateRef, Type } from '@angular/core';
import { EmbeddedViewRef } from '@angular/core';
import { DOCUMENT } from '@angular/common';
// Library
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
	private _IsKeyDownListenerActive: boolean = false;

	constructor(
		private _ApplicationRef: ApplicationRef,
		@Inject(DOCUMENT) private _Document: Document,
		private _GWCommon: GWCommon,
		// private _LoggingSvc: LoggingService,
	) { }

	/**
	 * Handles the keydown event.
	 * If the key pressed is ESC, stop the propagation and close the last modal if there are any.
	 * @param {KeyboardEvent} event - The keydown event.
	 */
	private handleKeyDown(event: KeyboardEvent): void {
		// We handle the ESC key here so that we can limit the closing or canceling
		// of a modal to the last one opened.
		if (event.key === 'Escape') {
			event.stopPropagation();
			if (this._ActiveModals.length > 0) {
				// Get the last modal in the active modals array
				const mLastModal = this._ActiveModals[this._ActiveModals.length - 1];
				if (mLastModal) {
					// Call the cancel method for the last modal
					mLastModal.modalComponentRef.instance.onCancel();
				}
			}
		}
	}

	/**
	 * Closes a modal for the specified key.
	 *
	 * @param {string} key - The key of the modal to close.
	 */
	public close(key: string) {
		const mContentObj = this._ActiveModals.find((obj: IContentObject) => obj.key.toUpperCase() === key.toUpperCase() as string);
		if (mContentObj !== undefined) {
			if (mContentObj.contentType === ContentType.Component) {
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
					this.logConsole(mMsg, 'Error');
				}
			}
			// remove and destroy the modal component
			this._ApplicationRef.detachView(mContentObj.modalComponentRef.hostView);
			mContentObj.modalComponentRef.destroy();
			this._ActiveModals = this._ActiveModals.filter(obj => obj !== mContentObj);
			// Add the keydown event listener for ESC key if not already added
			if (!this._IsKeyDownListenerActive) {
				this._Document.addEventListener('keydown', this.handleKeyDown.bind(this));
				this._IsKeyDownListenerActive = true; // Set the flag to true
			}
		}
	}

	/**
	 * Opens a modal dialog with either string, ng-template or a component.
	 *
	 * @param {IModalOptions} options - The options for the modal dialog.
	 * @return {void} 
	 */
	public open(options: IModalOptions): void {
		if (this._GWCommon.isNullOrEmpty(options.modalId)) {
			this.logConsole('options.modalId can not be null or blank', 'Error');
			// console.log('ModalService.open', options);
			return;
		}
		if (this._GWCommon.isNullOrEmpty(options.contentPayLoad) && typeof (options.contentPayLoad) !== 'function') {
			this.logConsole('Please set the contentPayLoad property', 'Error');
			return;
		}
		// resolve the ngContent
		const mResolvedNgContent = this.resolveNgContent(options.contentPayLoad);
		// first, create the child
		// eslint-disable-next-line @typescript-eslint/no-explicit-any
		let mNgContent: any = mResolvedNgContent;
		if (this._ContentType === ContentType.Component) {
			// get root nodes
			mNgContent = (<EmbeddedViewRef<unknown>>mResolvedNgContent.hostView).rootNodes;
		}
		// then create the dialog that will host it
		const mModalComponentRef: ComponentRef<ModalComponent> = createComponent(ModalComponent, {
			environmentInjector: this._ApplicationRef.injector,
			projectableNodes: [mNgContent], // pass the child here
		});
		mModalComponentRef.instance.setUp(options); // sets up UI properties (height, width, show components, etc.)

		this.setupModalCallbacks(options, mModalComponentRef.instance);
		// append to body, we will use platform document for this
		const mDialogElement = (<EmbeddedViewRef<unknown>>mModalComponentRef.hostView).rootNodes[0];
		// setup a ContentObject to add to the array
		const mContentObject = new ContentObject(options.modalId, this._ContentType, mModalComponentRef);
		if (this._ContentType === ContentType.Component) { // the payloadRef is only used when it's a component so destroy can be called in the this.close
			mContentObject.payloadRef = mResolvedNgContent;
		}

		// Add the keydown event listener for ESC key if not already added
		if (!this._IsKeyDownListenerActive) {
			this._Document.addEventListener('keydown', this.handleKeyDown.bind(this));
			this._IsKeyDownListenerActive = true; // Set the flag to true
		}

		// add the new modal to the array
		this._ActiveModals.push(mContentObject);

		// append the dialog element to the body :-)
		this._Document.body.append(mDialogElement);
		// attach the ModalComponentRef host view to the application view
		this._ApplicationRef.attachView(mModalComponentRef.hostView);
	}

	private logConsole(msg: string, level: string): void {
		const mMsg =
			this._GWCommon.getStackTrace().replace(new RegExp(' => ' + '$'), ':') +
			'\n  ' +
			msg;
		switch (level) {
			case 'Debug':
				console.debug(mMsg);
				break;
			case 'Error':
			case 'Fatal':
				console.error(mMsg);
				break;
			case 'Info':
				console.info(mMsg);
				break;
			case 'Warn':
				console.warn(mMsg);
				break;
			case 'Trace':
				console.trace(mMsg);
				break;
			case 'Success':
			default:
				console.log(mMsg);
				break;
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
	private resolveNgContent<T>(content: Content<T>): any {
		this._ContentType = ContentType.String;
		// eslint-disable-next-line @typescript-eslint/no-explicit-any
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
				this.logConsole('You have not set the options.buttons.closeButton.callbackMethod', 'Error');
			}
		}
		if (this._GWCommon.isFunction(modalOptions.buttons.okButton.callbackMethod)) {
			mModalComponentInstance.oKCallBackMethod = modalOptions.buttons.okButton.callbackMethod;
		} else {
			if (modalOptions.buttons.okButton.visible) {
				this.logConsole('You have not set the options.buttons.okButton.callbackMethod', 'Error');
			}
		}
	}
}
