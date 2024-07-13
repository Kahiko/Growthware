export interface IRegistrationInformation {
    accountChoices: string;
    addAccount: number;
    groups: string;
    id: number;
    roles: string;
    securityEntitySeqIdOwner: number;
}

export class registrationInformation implements IRegistrationInformation {
	public accountChoices: string = '';
	public addAccount: number = -1;
	public groups: string = '';
	public id = -1;
	public roles: string = '';
	public securityEntitySeqIdOwner: number = -1;
}