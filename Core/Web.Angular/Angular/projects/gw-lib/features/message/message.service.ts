import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  
  public get addModalId(): string {
    return 'addMessage'
  }

  public get editModalId(): string {
    return 'editMessage'
  }

  editAccount: string = '';
  editReason: string = '';

  constructor() { }
}
