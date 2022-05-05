
export interface IDynamicTableColumn {
  "allowSort": boolean,
  "canEdit": boolean,
  "editText": string,
  "isCheckbox": boolean,
  "isDeleteBtn": boolean ,
  "isEdit": boolean,
  "isEditKey": string,
  "label": string,
  "name": string,
  "showData": boolean,
  "type": string,
  "visible": boolean,
  "width": number
}

export interface IDynamicTableButton {
  "id": string,
  "name": string,
  "class": string,
  "text": string,
  "visible": boolean
}

export interface IDynamicTableConfiguration {
  "buttons": IDynamicTableButton[],
  "columns": IDynamicTableColumn[],
  "headingText": string,
  "maxHeadHeight": number,
  "maxTableRowHeight": number,
  "name": string,
  "numberOfRows": number,
  "orderByColumn": string,
  "showFirstRow": boolean,
  "showSecondRow": boolean,
  "showThirdRow": boolean,
  "showHeading": boolean,
  "showHelp": boolean,
  "showSearch": boolean
}
