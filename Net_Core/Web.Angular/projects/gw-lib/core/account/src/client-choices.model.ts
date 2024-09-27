export interface IClientChoices {
    account: string;
    action: string;
    securityEntityId: number;
    securityEntityName: string;
    recordsPerPage: number;

	colorScheme: string;
    evenRow: string;
    evenFont: string;
    oddRow: string;
    oddFont: string;
    background: string;
    headerRow: string;
    headerFont: string;
}

export class ClientChoices implements IClientChoices {
	account = 'Anonymous';
	action = '';
	securityEntityId = 1;
	securityEntityName = '';
	recordsPerPage = 10;
    colorScheme = 'Blue';
    evenRow = '#6699cc';
    evenFont = 'White';
    oddRow = '#b6cbeb';
    oddFont = 'Black';
    background = '#ffffff';
    headerRow = '#C7C7C7';
    headerFont = 'Black';
}
