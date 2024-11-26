import { Injectable } from '@angular/core';
import { ITotalRecords } from '@growthware/common/interfaces';
// Library
import { BaseService } from '@growthware/core/base/services';
// Feature
import { SelectedRow } from './selected-row.model';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService extends BaseService {
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

  constructor() { 
    super();
  }
}
