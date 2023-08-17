import { INameValuePare } from "@Growthware/shared/models";

export interface ISearchCriteria {
  searchColumns: Array<string>,
  sortColumns: Array<string>,
  pageSize: number,
  searchText: string,
  selectedPage: number
}

export class SearchCriteria implements ISearchCriteria {
  constructor(
    public searchColumns: Array<string>,
    public sortColumns: Array<string>,
    public pageSize: number,
    public searchText: string,
    public selectedPage: number,
  ) {}
}

export interface ISearchCriteriaNVP extends INameValuePare {
  name: string,
  payLoad: ISearchCriteria
}

export class SearchCriteriaNVP implements ISearchCriteriaNVP {

  constructor(public name: string, public payLoad: ISearchCriteria) {}
}


export interface ISearchResultsNVP extends INameValuePare {
  name: string,
  payLoad: {
    data: Array<any>
    totalRecords: number,
    searchCriteria: ISearchCriteria,
  }
}

export class SearchResultsNVP implements ISearchResultsNVP {

  constructor(
    public name: string,
    public payLoad: {
      data: Array<any>,
      totalRecords: number,
      searchCriteria: ISearchCriteria,
    }) {}
}
