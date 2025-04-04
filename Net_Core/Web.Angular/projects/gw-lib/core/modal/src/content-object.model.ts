import { ComponentRef, TemplateRef } from '@angular/core';
import { ContentType } from './content-type.enum';
import { ModalComponent } from './c/popup/modal.component';

export interface IContentObject {
  contentType: ContentType;                                         // The type of the content makes it easier to identify the content
  key: string;                                                      // Used to find the object in an array
  modalComponentRef: ComponentRef<ModalComponent>;                  // Reference to the modal component so it can be closed
  payloadRef: Text | TemplateRef<unknown> | ComponentRef<unknown>;  // Reference to the content
}

export class ContentObject implements IContentObject {
  public payloadRef: Text | TemplateRef<unknown> | ComponentRef<unknown> = document.createTextNode('');

  constructor(
    public key: string,
    public contentType: ContentType,
    public modalComponentRef: ComponentRef<ModalComponent>,
    payloadRef?: Text | TemplateRef<unknown> | ComponentRef<unknown>
  ) {
    if (payloadRef !== undefined) {
      this.payloadRef = payloadRef;
    }
  }
}
