import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SecurityEntityService {
  
  public get addModalId(): string {
    return 'addSecurityEntity'
  }

  public get editModalId(): string {
    return 'editSecurityEntity'
  }

  editAccount: string = '';
  editReason: string = '';

  constructor() { }
}
