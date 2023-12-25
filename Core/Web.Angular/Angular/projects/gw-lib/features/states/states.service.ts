import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { GWCommon } from '@Growthware/common-code';
import { LoggingService } from '@Growthware/features/logging';
import { SearchService } from '@Growthware/features/search';
// Feature
import { IStateProfile } from './state-profile.model';

@Injectable({
  providedIn: 'root'
})
export class StatesService {

  private _ApiName: string = 'GrowthwareState/';
  private _Api_GetProfile: string = '';
  private _Api_Save: string = '';

  readonly addEditModalId: string = 'addEditAccountModal';

  modalReason: string = '';
  selectedRow: any = {};

  constructor(
    private _GWCommon: GWCommon,
    private _HttpClient: HttpClient,
    private _LoggingSvc: LoggingService,
    private _SearchSvc: SearchService
  ) { 
    this._Api_GetProfile = this._GWCommon.baseURL + this._ApiName + 'GetProfile';
    this._Api_Save = this._GWCommon.baseURL + this._ApiName + 'Save';
  }

  public async getState(state: string): Promise<IStateProfile> {
    const mQueryParameter: HttpParams = new HttpParams().append('state', state);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };    
    return new Promise<IStateProfile>((resolve, reject) => {
      this._HttpClient.get<IStateProfile>(this._Api_GetProfile, mHttpOptions).subscribe({
        next: (response: any) => {
          resolve(response);
        },
        error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'getFunction');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }

  public async saveState(stateProfile: IStateProfile): Promise<boolean> {
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    }
    return new Promise<boolean>((resolve, reject) => {
      this._HttpClient.post<boolean>(this._Api_Save, stateProfile, mHttpOptions).subscribe({
        next: (response: any) => {
          const mSearchCriteria = this._SearchSvc.getSearchCriteria("States"); // from SearchStatesComponent (this.configurationName)
          if(mSearchCriteria != null) {
            this._SearchSvc.setSearchCriteria("States", mSearchCriteria);
          }
          resolve(response);
        }
        , error: (error: any) => {
          this._LoggingSvc.errorHandler(error, 'StatesService', 'saveState');
          reject('Failed to call the API');
        }
      })
    })
  }
}
