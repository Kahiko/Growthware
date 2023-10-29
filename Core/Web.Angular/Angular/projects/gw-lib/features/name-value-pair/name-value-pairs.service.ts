import { Injectable } from '@angular/core';
// import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
// import { GWCommon } from '@Growthware/common-code';
// import { LoggingService } from '@Growthware/features/logging';
// import { SearchService } from '@Growthware/features/search';
// Feature
import { INvpParentProfile } from './name-value-pair-parent-profile.model';

@Injectable({
  providedIn: 'root'
})
export class NameValuePairService {

  public nameNVPParrentRow!: INvpParentProfile;
  public modalIdNVPParrent: string = 'AddOrEditNVPParrent';


  constructor(
    // private _GWCommon: GWCommon,
    // private _HttpClient: HttpClient, 
    // private _LoggingSvc: LoggingService,
    // private _SearchSvc: SearchService
  ) { }

  setNameNVPParrentRow(row: INvpParentProfile): void {
    this.nameNVPParrentRow = JSON.parse(JSON.stringify(row));
  }


}
