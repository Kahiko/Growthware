export interface ISearchCriteria {
  columns: string,
  orderByColumn: string,
  orderByDirection: string,
  pageSize: number,
  selectedPage: number,
  tableOrView: string,
  whereClause: string
}

export class SearchCriteria implements ISearchCriteria {
  public tableOrView: string = '';

  constructor(
    public columns: string,
    public orderByColumn: string,
    public orderByDirection: string,
    public pageSize: number,
    public selectedPage: number,
    public whereClause: string
  ) {}
}
