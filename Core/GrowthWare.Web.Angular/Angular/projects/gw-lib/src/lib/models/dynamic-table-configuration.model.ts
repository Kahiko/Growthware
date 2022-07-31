import { ICallbackButton, CallbackButton, IDynamicTableColumn } from './public-api';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';

export interface IDynamicTableConfiguration {
  "buttons": ICallbackButton[],
  "columns": IDynamicTableColumn[],
  "captionText": string,
  "maxHeadHeight": number,
  "maxTableRowHeight": number,
  "name": string,
  "numberOfRows": number,
  "showFirstRow": boolean,
  "showThirdRow": boolean,
  "showHelp": boolean,
  "showSearch": boolean,
  "tableHeight": number
}

/**
 * Represents the implementation of IDynamicTableConfig
 *
 * @export
 * @class DynamicTableConfiguration
 */
 export class DynamicTableConfiguration {
  public buttons: ICallbackButton[] = [];
  public columns: IDynamicTableColumn[];
  public headingText: string;
  public maxHeadHeight: number;
  public maxTableRowHeight: number;
  public name: string;
  public numberOfRows: number;
  public showFirstRow: boolean;
  public showThirdRow: boolean;
  public showHeading: boolean;
  public showHelp: boolean;
  public showSearch: boolean;
  public rowClick?: (arg: any | any[]) => void;
  public rowDoubleClick?: (arg: any | any[]) => void;

  constructor(
    buttons: ICallbackButton[] = [],
    columns: IDynamicTableColumn[],
    headingText: string = '',
    maxHeadHeight: number = 32,
    maxTableRowHeight: number = 400,
    name: string,
    numberOfRows: number = 4,
    showFirstRow: boolean = false,
    showThirdRow: boolean = false,
    showHeading: boolean = false,
    showHelp: boolean = false,
    showSearch: boolean = false,
    private _GWCommon: GWCommon,
  ) {
    this.headingText = headingText;
    this.maxHeadHeight = maxHeadHeight;
    this.maxTableRowHeight = maxTableRowHeight;
    this.name = name;
    this.numberOfRows = numberOfRows;
    this.showFirstRow = showFirstRow;
    this.showThirdRow = showThirdRow;
    this.showHeading = showHeading;
    this.showHelp = showHelp;
    this.showSearch = showSearch;

    if(this._GWCommon.isNullOrUndefined(buttons) || buttons.length === 0) {
      const mDefaultNameId = name + '_AddBtn';
      let mButton:ICallbackButton = new CallbackButton('Add', mDefaultNameId, mDefaultNameId)
      this.buttons.push(mButton);
    } else {
      // ensure number of buttons not greater than 5
      if(buttons.length <= 5) {
        this.buttons = buttons;
      } else {
        this.buttons = buttons.slice(0, 4);
      }
    }

    if(!this._GWCommon.isNullOrUndefined(columns) && columns.length > 0) {
      let result = columns.filter(o => o.visible === true);
      if(this._GWCommon.isNullOrUndefined(result) || result.length === 0) {
        throw new Error("At least 1 column in the columns must be visible");
      } else {
        this.columns = columns;
      }
    } else {
      throw new Error("You must pass a populated array of column information");
    }
  }
}
