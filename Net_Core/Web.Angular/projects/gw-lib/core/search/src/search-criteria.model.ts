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
  ) { }
}

export interface ISearchCriteriaNVP {
  name: string,
  payLoad: ISearchCriteria
}

export class SearchCriteriaNVP implements ISearchCriteriaNVP {

  constructor(public name: string, public payLoad: ISearchCriteria) { }
}


export interface ISearchResultsNVP {
  name: string,
  payLoad: {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    data: Array<any>
    totalRecords: number,
    searchCriteria: ISearchCriteria,
  }
}

export class SearchResultsNVP implements ISearchResultsNVP {

  constructor(
    public name: string,
    public payLoad: {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      data: Array<any>,
      totalRecords: number,
      searchCriteria: ISearchCriteria,
    }) { }
}
