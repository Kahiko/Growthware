export class ClientChoices {
  public constructor(
    public account: string,
    public alternatingRowBackColor: string,
    public backColor: string,
    public colorScheme: string,
    public favoriteAction: string,
    public headColor: string,
    public headerForeColor: string,
    public leftColor: string,
    public recordsPerPage: number,
    public rowBackColor: string,
    public securityEntityID: number,
    public securityEntityName: string,
    public subHeadColor: string
  ) {
      this.account = account;
      this.alternatingRowBackColor = alternatingRowBackColor;
      this.backColor = backColor;
      this.colorScheme = colorScheme;
      this.favoriteAction = favoriteAction;
      this.headColor = headColor;
      this.headerForeColor = headerForeColor;
      this.leftColor = leftColor;
      this.recordsPerPage = recordsPerPage;
      this.rowBackColor = rowBackColor;
      this.securityEntityID = securityEntityID;
      this.securityEntityName = securityEntityName;
      this.subHeadColor = subHeadColor;
  }
}
