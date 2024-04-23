export interface IClientChoices {
    account: string,
	accountName: string,
	action: string,
	alternatingRowBackColor: string,
	backColor: string,
	colorScheme: string,
	headerForeColor: string,
	headColor: string,
	leftColor: string,
	recordsPerPage: number,
	rowBackColor: string,
	securityEntityID: number,
	securityEntityName: string,
	subHeadColor: string,
}

export class ClientChoices implements IClientChoices {
	account = 'Anonymous';
	accountName = 'Anonymous';
	action = '';
	alternatingRowBackColor = '';
	backColor = '';
	colorScheme = 'Blue';
	headerForeColor = '';
	headColor = '';
	leftColor = '';
	recordsPerPage = 10;
	rowBackColor = '';
	securityEntityID = 1;
	securityEntityName = '';
	subHeadColor = '';
}
