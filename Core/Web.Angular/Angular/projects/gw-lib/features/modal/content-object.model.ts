export interface IContentObject {
  isComponent: boolean;
  key: string;
  modalComponentRef: any;
  nativeElement: any;
  payloadRef: any;
}

export class ContentObject implements IContentObject {

  payloadRef: any = null;

  constructor(
    public key: string,
    public isComponent: boolean,
    public modalComponentRef: any,
    public nativeElement: any,
  ) { }
}
