export interface IDynamicTableColumn {
  "allowSearch": boolean,
  "allowSort": boolean,
  "canEdit": boolean,
  "direction": "asc" | "desc"
  "editText": string,
  "outputType": string, // button, checkbox, text
  "isPrimaryKey": string,
  "label": string,
  "name": string,
  "searchSelected": boolean,
  "showData": boolean,
  "sortSelected": boolean,
  "type": string,
  "visible": boolean,
  "width": number
}
