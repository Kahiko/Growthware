import { Injectable } from '@angular/core';
// import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
// import { GWCommon } from '@Growthware/common-code';
// import { LoggingService } from '@Growthware/features/logging';
// import { SearchService } from '@Growthware/features/search';
// Feature
import { INvpParentProfile } from './name-value-pair-parent-profile.model';
import { INvpChildProfile } from './name-value-pair-child-profile.model';

@Injectable({
  providedIn: 'root'
})
export class NameValuePairService {

  public nvpParentRow!: INvpParentProfile;
  public nvpChildRow!: INvpChildProfile;
  public modalIdNVPParrent: string = 'AddOrEditNVPParrent';
  public modalIdNVPChild: string = 'AddOrEditNVPChild';


  constructor(
    // private _GWCommon: GWCommon,
    // private _HttpClient: HttpClient, 
    // private _LoggingSvc: LoggingService,
    // private _SearchSvc: SearchService
  ) { }

  setNameValuePairParrentRow(row: INvpParentProfile): void {
    this.nvpParentRow = JSON.parse(JSON.stringify(row));
  }

  setNameValuePairDetailRow(row: INvpChildProfile): void {
    this.nvpChildRow = JSON.parse(JSON.stringify(row));
  }

}
