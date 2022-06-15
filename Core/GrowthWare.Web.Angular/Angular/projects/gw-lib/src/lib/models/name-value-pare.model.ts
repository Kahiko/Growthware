import { ISearchCriteria } from "./search-criteria.model";

export interface INameValuePare {
  name: string,
  payLoad: any
}

export class DataNVP implements INameValuePare {

  constructor(public name: string, public payLoad: Array<any>) {}
}

export class SearchCriteriaNVP implements INameValuePare {

  constructor(public name: string, public payLoad: ISearchCriteria) {}
}

export class SearchTotalRecordsNVP implements INameValuePare {

  constructor(public name: string, public payLoad: number) {}
}

export class SearchResultsNVP implements INameValuePare {

  constructor(public name: string, public payLoad: {searchCriteria?: ISearchCriteria, data: Array<any>}) {}
}
