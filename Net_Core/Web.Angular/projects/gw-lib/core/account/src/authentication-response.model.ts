export interface IAuthenticationResponse {
  account: string;
  created: string;
  email: string;
  firstName: string;
  id: number;
  isSystemAdmin: boolean;
  isVerified: boolean;
  jwtToken: string;
  lastName: string;
  location: string;
  middleName: string;
  preferredName: string;
  status: number;
  timeZone: number;
  updated: string;
}

export class AuthenticationResponse implements IAuthenticationResponse {
	account = '';
	created = '';
	email = '';
	firstName = '';
	id = -1;
	isSystemAdmin = false;
	isVerified = false;
	jwtToken = '';
	lastName = '';
	location = '';
	middleName = '';
	preferredName = '';
	status = 4;
	timeZone = -10;
	updated = '';

	constructor() {}
}