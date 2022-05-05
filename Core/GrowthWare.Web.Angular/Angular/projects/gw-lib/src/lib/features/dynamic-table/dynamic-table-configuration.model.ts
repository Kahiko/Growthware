import { throwError } from 'rxjs';

import { IDynamicTableButton, IDynamicTableColumn } from './dynamic-table.interfaces';
import { DynamicTableButton } from './dynamic-table-button.model';
import { Common } from '../../common';

/**
 * Represents the implementation of IDynamicTableConfig
 *
 * @export
 * @class DynamicTableConfig
 */
export class DynamicTableConfig {
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

  constructor(
    buttons: IDynamicTableButton[] = [],
    columns: IDynamicTableColumn[],
    headingText: string = '',
    maxHeadHeight: number = 32,
    maxTableRowHeight: number = 400,
    name: string,
    numberOfRows: number = 4,
    orderByColumn: string,
    showFirstRow: boolean = false,
    showSecondRow: boolean = false,
    showThirdRow: boolean = false,
    showHeading: boolean = false,
    showHelp: boolean = false,
    showSearch: boolean = false
  ) {
    // this.buttons = buttons;
    // this.columns = columns;
    this.headingText = headingText;
    this.maxHeadHeight = maxHeadHeight;
    this.maxTableRowHeight = maxTableRowHeight;
    this.name = name;
    this.numberOfRows = numberOfRows;
    this.orderByColumn = orderByColumn;
    this.showFirstRow = showFirstRow;
    this.showSecondRow = showSecondRow;
    this.showThirdRow = showThirdRow;
    this.showHeading = showHeading;
    this.showHelp = showHelp;
    this.showSearch = showSearch;

    if(Common.isNullOrUndefined(buttons) || buttons.length === 0) {
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

    if(!Common.isNullOrUndefined(columns) && columns.length > 0) {
      let result = columns.filter(o => o.visible === true);
      if(Common.isNullOrUndefined(result) || result.length === 0) {
        throw new Error("At least 1 column in the columns must be visible");
      } else {
        this.columns = columns;
      }
    } else {
      throw new Error("You must pass a populated array of column information");
    }
  }
}
