import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'gw-frontend-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  private _Skin: BehaviorSubject<string> = new BehaviorSubject<string>('default');

  readonly skin = this._Skin.asObservable();
  // skin = 'default';
  title = 'gw-frontend';

  constructor() {}
}
