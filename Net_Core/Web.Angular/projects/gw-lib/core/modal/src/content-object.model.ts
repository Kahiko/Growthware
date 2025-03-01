import { ContentType } from './content-type.enum';
import { ComponentRef, TemplateRef } from '@angular/core';
import { ModalComponent } from './c/popup/modal.component';

export interface IContentObject<T> {
  contentType: ContentType;
  key: string;
  modalComponentRef: ComponentRef<ModalComponent>;
  payloadRef: ComponentRef<T> | TemplateRef<T> | null;
}

export class ContentObject<T> implements IContentObject<T> {

  payloadRef: ComponentRef<T> | TemplateRef<T> | null = null;

  constructor(
    public key: string,
    public contentType: ContentType,
    public modalComponentRef: ComponentRef<ModalComponent>,
  ) { }
}
