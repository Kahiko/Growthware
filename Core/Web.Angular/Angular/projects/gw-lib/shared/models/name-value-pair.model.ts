export interface INameValuePair {
    name: string,
    payLoad: any
  }
  
  export class DataNVP implements INameValuePair {
  
    constructor(public name: string, public payLoad: Array<any>) {}
  }
  
  export class SearchTotalRecordsNVP implements INameValuePair {
  
    constructor(public name: string, public payLoad: number) {}
  }
