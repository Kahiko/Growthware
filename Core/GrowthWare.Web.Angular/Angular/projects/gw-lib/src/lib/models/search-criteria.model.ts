export interface ISearchCriteria {
  searchColumns: Array<string>,
  sortColumnInfo: Array<string>,
  pageSize: number,
  searchText: string,
  selectedPage: number
}

export class SearchCriteria implements ISearchCriteria {
  constructor(
    public searchColumns: Array<string>,
    public sortColumnInfo: Array<string>,
    public pageSize: number,
    public searchText: string,
    public selectedPage: number,
  ) {}
}

