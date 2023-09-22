export interface IContentObject {
  isComponent: boolean;
  key: string;
  modalComponentRef: any;
  payloadRef: any;
  nativeElement: any;
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
