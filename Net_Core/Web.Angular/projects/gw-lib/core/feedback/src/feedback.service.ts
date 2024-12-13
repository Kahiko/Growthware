import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
// Library
import { BaseService } from '@growthware/core/base/services';
import { GWCommon } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { SearchService } from '@growthware/core/search';
// Feature
import { SelectedRow } from './selected-row.model';
import { IFeedback } from './feedback.model';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService extends BaseService {
  private _ApiName: string = 'GrowthwareFeedback/';
  private _Api_GetFeedbackForEdit: string = '';
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
    this._Api_GetFeedbackForEdit = this._GWCommon.baseURL + this._ApiName + 'GetFeedbackForEdit';
  }

  getFeedback(feedbackId: number): Promise<IFeedback> {
    const mQueryParameter: HttpParams = new HttpParams().append('feedbackId', feedbackId);
    const mHttpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      params: mQueryParameter,
    };
    return new Promise<IFeedback>((resolve, reject) => {
      this._HttpClient.get<IFeedback>(this._Api_GetFeedbackForEdit, mHttpOptions).subscribe({
        next: (response: IFeedback) => {
          resolve(response);
        },
        error: (error) => {
          this._LoggingSvc.errorHandler(error, 'FunctionService', 'getFunction');
          reject('Failed to call the API');
        },
        // complete: () => {}
      });
    });
  }
}
