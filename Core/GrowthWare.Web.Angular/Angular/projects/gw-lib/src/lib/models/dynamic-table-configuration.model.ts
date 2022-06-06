import { IDynamicTableButton, DynamicTableButton } from './dynamic-table-button.model';
import { IDynamicTableColumn } from './dynamic-table-column.model';
import { GWCommon } from '@Growthware/Lib/src/lib/common-code';

export interface IDynamicTableConfiguration {
  "buttons": IDynamicTableButton[],
  "columns": IDynamicTableColumn[],
  "captionText": string,
  "maxHeadHeight": number,
  "maxTableRowHeight": number,
  "name": string,
  "numberOfRows": number,
  "orderByColumn": string,
  "showFirstRow": boolean,
  "showThirdRow": boolean,
  "showHelp": boolean,
  "showSearch": boolean,
  "tableHeight": number,
  "tableOrView": string
}


/**
 * Represents the implementation of IDynamicTableConfig
 *
 * @export
 * @class GWLibDynamicTableConfigModel
 */
 export class GWLibDynamicTableConfigModel {
  public buttons: IDynamicTableButton[] = [];
  public columns: IDynamicTableColumn[];
  public headingText: string;
  public maxHeadHeight: number;
  public maxTableRowHeight: number;
  public name: string;
  public numberOfRows: number;
  public orderByColumn: string;
  public showFirstRow: boolean;
  public showSecondRow: boolean;
  public showThirdRow: boolean;
  public showHeading: boolean;
  public showHelp: boolean;
  public showSearch: boolean;
  public tableOrView: string;

  constructor(
    buttons: IDynamicTableButton[] = [],
    columns: IDynamicTableColumn[],
    headingText: string = '',
    maxHeadHeight: number = 32,
    maxTableRowHeight: number = 400,
    name: string,
    numberOfRows: number = 4,
    orderByColumn: string,
    tableOrView: string,
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
    this.orderByColumn = orderByColumn;
    this.showFirstRow = showFirstRow;
    this.showThirdRow = showThirdRow;
    this.showHeading = showHeading;
    this.showHelp = showHelp;
    this.showSearch = showSearch;
    this.tableOrView = tableOrView;

    if(this._GWCommon.isNullOrUndefined(buttons) || buttons.length === 0) {
      let mDefaultNameId = name + '_AddBtn';
      let mButton:IDynamicTableButton = new DynamicTableButton(mDefaultNameId, mDefaultNameId);
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
