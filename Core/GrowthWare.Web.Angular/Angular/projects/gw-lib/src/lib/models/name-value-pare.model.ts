export interface INameValuePare {
  name: string,
  payLoad: any
}

export class DataNVP implements INameValuePare {

  constructor(public name: string, public payLoad: Array<any>) {}
}

export class SearchTotalRecordsNVP implements INameValuePare {

  constructor(public name: string, public payLoad: number) {}
}

