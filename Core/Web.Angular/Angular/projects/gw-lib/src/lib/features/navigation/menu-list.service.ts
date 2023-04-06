import { Injectable } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MenuListService {
  private _ShowNavText = new BehaviorSubject<boolean>(false);

  public currentUrl = new BehaviorSubject<string>('');
  readonly showNavText$ = this._ShowNavText.asObservable();

  constructor(private router: Router) {
    this.router.events.subscribe({
      next: (event: any) => {
        if (event instanceof NavigationEnd) {
          this.currentUrl.next(event.urlAfterRedirects);
        }
      },
    });
  }

  setShowNavText(value: boolean): void {
    this._ShowNavText.next(value);
  }
}
