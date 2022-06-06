export interface ISecurityInfo {
  "mayAdd": Boolean
  "mayEdit": Boolean
  "mayDelete": Boolean
  "mayView": Boolean
}

export class SecurityInfo implements ISecurityInfo {

  constructor(
    public mayAdd: boolean = false,
    public mayEdit: boolean = false,
    public mayDelete: boolean = false,
    public mayView: boolean = false
  ) {}
}
