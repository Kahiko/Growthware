export interface IClientChoices {
    account: string,
	alternatingRowBackColor: string,
	backColor: string,
	colorScheme: string,
	favoriteAction: string,
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
	alternatingRowBackColor = '';
	backColor = '';
	colorScheme = 'Blue';
	favoriteAction = '';
	headerForeColor = '';
	headColor = '';
	leftColor = '';
	recordsPerPage = 10;
	rowBackColor = '';
	securityEntityID = 1;
	securityEntityName = '';
	subHeadColor = '';
}
