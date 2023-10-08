import { ContentType } from "./content-type.enum";

export interface IContentObject {
  contentType: ContentType;
  key: string;
  modalComponentRef: any;
  payloadRef: any;
}

export class ContentObject implements IContentObject {

  payloadRef: any = null;

  constructor(
    public key: string,
    public contentType: ContentType,
    public modalComponentRef: any,
  ) { }
}
