import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
// Library
import { BaseService } from '@growthware/core/base/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { SearchService } from '@growthware/core/search';
// Feature
import { SelectedRow } from './selected-row.model';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService extends BaseService {
  private _ApiName: string = 'GrowthwareFeedback/';
  private _Reason: string = '';

  readonly addEditModalId: string = 'addEditFeedbackModal';
  modalReason: string = '';
  selectedRow: SelectedRow = new SelectedRow();

  public get reason(): string {
		return this._Reason;
	}
	public set reason(value: string) {
		this._Reason = value;
	}

  constructor(
		private _GWCommon: GWCommon,
		private _HttpClient: HttpClient,
		private _LoggingSvc: LoggingService,
		private _SearchSvc: SearchService,    
  ) { 
    super();
  }
}
