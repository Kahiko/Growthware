import { ComponentRef, TemplateRef } from '@angular/core';
import { ContentType } from './content-type.enum';
import { ModalComponent } from './c/popup/modal.component';

export interface IContentObject<T = unknown> {
  contentType: ContentType;                               // The type of the content makes it easier to identify the content
  key: string;                                            // Used to find the object in an array
  modalComponentRef: ComponentRef<ModalComponent>;        // Reference to the modal component so it can be closed
  payloadRef: string | TemplateRef<T> | ComponentRef<T>;  // Reference to the content
}

export class ContentObject<T = unknown> implements IContentObject<T> {
  public payloadRef: string | TemplateRef<T> | ComponentRef<T> = '';

  constructor(
    public key: string,
    public contentType: ContentType,
    public modalComponentRef: ComponentRef<ModalComponent>,
    payloadRef?: string | TemplateRef<T> | ComponentRef<T>
  ) {
    if (payloadRef !== undefined) {
      this.payloadRef = payloadRef;
    }
  }
}
