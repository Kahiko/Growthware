export interface ISecurityInfo {
    mayView: boolean;
    mayAdd: boolean;
    mayEdit: boolean;
    mayDelete: boolean;
}

export class SecurityInfo implements ISecurityInfo {

	constructor(public mayAdd: boolean = false, public mayEdit: boolean = false, public mayDelete: boolean = false, public mayView: boolean = false) { }
}
