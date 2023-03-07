export interface IContentObject {
  componentRef: any;
  isComponent: boolean;
  key: string;
  value: any;
}

export class ContentObject implements IContentObject {

  constructor(
    public componentRef: any,
    public isComponent: boolean,
    public key: string,
    public value: any,
  ) { }
}
