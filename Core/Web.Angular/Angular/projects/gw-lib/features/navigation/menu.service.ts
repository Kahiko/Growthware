import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router, NavigationEnd } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
// Library
import { DataService } from '@Growthware/shared/services';
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
// Feature
import { INavLink } from './nav-link.model';
import { MenuType } from './menu-type.model';

@Injectable({
  providedIn: 'root',
})
export class MenuService {

  private _ApiName: string = 'GrowthwareAccount/';
  private _Api_GetMenuItems: string = '';
  private _BaseURL: string = '';

  private _ShowNavText = new BehaviorSubject<boolean>(true); // Sets the inital value in all controls

  public currentUrl = new BehaviorSubject<string>('');
  readonly showNavText$ = this._ShowNavText.asObservable();

  constructor(
    private _DataSvc: DataService,
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
    private router: Router
  ) {
    this._BaseURL = this._GWCommon.baseURL;
    this._Api_GetMenuItems = this._BaseURL + this._ApiName + 'GetMenuItems';
    this.router.events.subscribe({
      next: (event: any) => {
        if (event instanceof NavigationEnd) {
          this.currentUrl.next(event.urlAfterRedirects);
        }
      },
    });
  }

  public getNavLinks(menuType: MenuType, configuarionName: string): void {
    const mQueryParameter: HttpParams = new HttpParams()
      .set('menuType', menuType);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter
    };
    this._HttpClient.get<INavLink[]>(this._Api_GetMenuItems, mHttpOptions).subscribe({
      next: (response) => {
        this._DataSvc.notifyDataChanged(configuarionName, response);
      },
      error: (error) => {
        this._LoggingSvc.errorHandler(error, 'MenuService', 'getNavLinks');
      },
      complete: () => {
        // here as example
      }
    })
  }

  getShowNavText(): boolean {
    return this._ShowNavText.getValue();
  }

  setShowNavText(value: boolean): void {
    this._ShowNavText.next(value);
  }
}
