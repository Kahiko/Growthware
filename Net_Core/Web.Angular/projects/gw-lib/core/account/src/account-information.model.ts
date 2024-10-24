// Library
import { IClientChoices, ClientChoices } from '@growthware/core/clientchoices';
// Feature
import { IAuthenticationResponse, AuthenticationResponse } from './authentication-response.model';

export interface IAccountInformation {
    authenticationResponse: IAuthenticationResponse;
    clientChoices: IClientChoices;
}

export class AccountInformation implements IAccountInformation {
	authenticationResponse: IAuthenticationResponse = new AuthenticationResponse();
	clientChoices: IClientChoices = new ClientChoices();

	constructor() {
	}
}
