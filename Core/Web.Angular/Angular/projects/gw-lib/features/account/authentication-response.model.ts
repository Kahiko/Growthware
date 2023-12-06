export interface IAuthenticationResponse {
  account: string,
  derivedRoles: string[],
  email: string,
  firstName: string,
  lastName: string,
  middleName: string,
  preferredName: string,
  status: number,
  isSystemAdmin: boolean,
  isVerified: boolean,
  jwtToken: string
}

export class AuthenticationResponse implements IAuthenticationResponse {
  account = 'Anonymous';
  derivedRoles = ['authenticated'];
  email = '';
  firstName = '';
  lastName = '';
  middleName = '';
  preferredName = '';
  status = 4;
  isSystemAdmin = true;
  isVerified = true;
  jwtToken = '';

  constructor() {}
}