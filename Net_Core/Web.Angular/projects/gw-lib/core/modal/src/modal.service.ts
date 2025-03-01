import {
	ApplicationRef,
	Component,
	ComponentRef,
	createComponent,
	Inject,
	Injectable,
	EmbeddedViewRef,
	TemplateRef,
	Type
} from '@angular/core';
import { DOCUMENT } from '@angular/common';
// Library
import { GWCommon } from '@growthware/common/services';
// Features
import { ContentObject, IContentObject } from './content-object.model';
import { ContentType } from './content-type.enum';
import { ModalComponent } from './c/popup/modal.component';
import { IModalOptions } from './modal-options.model';

type Content<T> = string | TemplateRef<T> | Type<T>;

/**
 * Modal Service for creating dynamic, flexible modal dialogs
 * 
 * @description
 * Supports three primary content types:
 * - Plain text strings
 * - Angular Template References
 * - Angular Components
 * 
 * @example
 * // Opening a simple text modal
 * this._ModalService.open({
 *   modalId: 'confirmDelete',
 *   headerText: 'Confirm Deletion',
 *   contentPayLoad: 'Are you sure you want to delete this item?',
 *   buttons: {...}
 * });
 * 
 * @example
 * // Opening a template modal with initial data
 * this._ModalService.open({
 *   modalId: 'editUser',
 *   headerText: 'Edit User',
 *   contentPayLoad: this.editTemplate,
 *   initialData: { user: currentUser },
 *   buttons: {...}
 * });
 * 
 * @example
 * // Opening a component modal
 * this._ModalService.open({
 *   modalId: 'accountDetails',
 *   headerText: 'Account Details',
 *   contentPayLoad: AccountDetailsComponent,
 *   buttons: {...}
 * });
 * 
 * @remarks
 * Key Features and Best Practices:
 * 
 * 1. Modal Identification
 * - Always provide a unique `modalId`
 * - `modalId` is case-insensitive
 * 
 * 2. Content Types
 * - Supports string, TemplateRef, and Component payloads
 * - Use `initialData` only with TemplateRef
 * 
 * 3. Button Configuration
 * - Customize buttons via `buttons` property
 * - Can show/hide cancel, close, and OK buttons
 * 
 * 4. Window Sizing
 * - Control modal size using `windowSize`
 * - Can be a preset number or custom IWindowSize
 * 
 * 5. Callbacks
 * - Use `onOk`, `onCancel` for handling modal interactions
 * - `returnData` can be passed back through these callbacks
 */
@Injectable({
	providedIn: 'root'
})
export class ModalService {

	private _ActiveModals: IContentObject<ComponentRef<unknown> | TemplateRef<unknown> | null>[] = [];
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
	 * @private
	 * 
	 * @memberof ModalService
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
	 * Closes a specific modal by its modalId
	 * 
	 * @param {string} key - The modalId of the modal to close
	 * 
	 * @example
	 * // Close a modal with a specific ID
	 * this._ModalService.close('confirmDelete');
	 * 
	 * @memberof ModalService
	 */
	public close(key: string) {
		const mContentObj = this._ActiveModals.find((obj: IContentObject<unknown>) => obj.key.toUpperCase() === key.toUpperCase() as string);
		if (mContentObj !== undefined) {
			if (mContentObj.contentType === ContentType.Component) {
				try {
					if (mContentObj.payloadRef instanceof EmbeddedViewRef) {
						// destroy child
						this._ApplicationRef.detachView(mContentObj.payloadRef.rootNodes[0]);
					} else if (mContentObj.payloadRef instanceof ComponentRef) {
						// Handle the case when payloadRef is a ComponentRef
						// You might want to throw an error or set payloadRef to null
						throw new Error('mContentObj.payloadRef is not an EmbeddedViewRef');
					}
					if (mContentObj.payloadRef instanceof ComponentRef) {
						mContentObj.payloadRef.destroy();
					}
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
	 * 
	 * @memberof ModalService
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
		if (this._ContentType === ContentType.Component && mResolvedNgContent instanceof ComponentRef) {
			mNgContent = ((mResolvedNgContent as ComponentRef<Component>).hostView as EmbeddedViewRef<unknown>).rootNodes;
		}
		// then create the dialog that will host it
		const mModalComponentRef: ComponentRef<ModalComponent> = createComponent(ModalComponent, {
			environmentInjector: this._ApplicationRef.injector,
			projectableNodes: [mNgContent], // pass the child here
		});
		// const mContentObject = new ContentObject(options.modalId, this._ContentType, mModalComponentRef);
		const mContentObject: IContentObject<TemplateRef<unknown> | ComponentRef<unknown> | null> = new ContentObject(options.modalId, this._ContentType, mModalComponentRef);
		if (this._ContentType === ContentType.Component) {
			if (typeof mResolvedNgContent === 'object' && mResolvedNgContent instanceof ComponentRef) {
				mContentObject.payloadRef = mResolvedNgContent as ComponentRef<TemplateRef<unknown> | ComponentRef<unknown> | null>;
			} else {
				// Handle the case when mResolvedNgContent is of type Text
				// You might want to throw an error or set payloadRef to null
				throw new Error('mResolvedNgContent is not a ComponentRef or TemplateRef');
			}
		}
		mModalComponentRef.instance.setUp(options); // sets up UI properties (height, width, show components, etc.)
		this.setupModalCallbacks(options, mModalComponentRef.instance);
		// append to body, we will use platform document for this
		const mDialogElement = (<EmbeddedViewRef<unknown>>mModalComponentRef.hostView).rootNodes[0];
		// add the new modal to the array
		this._ActiveModals.push(mContentObject);

		// append the dialog element to the body :-)
		this._Document.body.append(mDialogElement);
		// attach the ModalComponentRef host view to the application view
		this._ApplicationRef.attachView(mModalComponentRef.hostView);
	}

	/**
	 * Logs a message to the console with an optional log type
	 * 
	 * @param {string} message - The message to log
	 * @param {string} [type='Log'] - The type of log (e.g., 'Error', 'Warn', 'Log')
	 * @private
	 * 
	 * @memberof ModalService
	 */
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
	 * 
	 * @memberof ModalService
	 */
	private resolveNgContent<T>(content: Content<T>): Text | ComponentRef<Component> | TemplateRef<T> {
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
	 * 
	 * @memberof ModalService
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
