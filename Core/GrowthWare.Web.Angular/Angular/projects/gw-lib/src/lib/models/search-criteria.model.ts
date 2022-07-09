export interface ISearchCriteria {
  columnInfo: Array<string>,
  pageSize: number,
  searchText: string,
  selectedPage: number
}

export class SearchCriteria implements ISearchCriteria {
  constructor(
    public columnInfo: Array<string>,
    public pageSize: number,
    public searchText: string,
    public selectedPage: number,
  ) {}
}

