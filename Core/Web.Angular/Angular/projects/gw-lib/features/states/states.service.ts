import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StatesService {
  public get addModalId(): string {
    return 'addState'
  }

  public get editModalId(): string {
    return 'editState'
  }

  editReason: string = '';
  editRow: any = {};

  constructor() { }
}
