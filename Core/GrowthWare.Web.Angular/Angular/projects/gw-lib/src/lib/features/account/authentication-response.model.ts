export interface IAuthenticationResponse {
  account: string,
  derivedRoles: string[],
  email: string,
  firstName: string,
  lastName: string,
  middleName: string,
  preferredName: string,
  isVerified: true,
  jwtToken: string
}
