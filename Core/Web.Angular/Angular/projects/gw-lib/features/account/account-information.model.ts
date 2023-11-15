import { IAuthenticationResponse, AuthenticationResponse } from './authentication-response.model';
import { IClientChoices, ClientChoices } from './client-choices.model';

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
