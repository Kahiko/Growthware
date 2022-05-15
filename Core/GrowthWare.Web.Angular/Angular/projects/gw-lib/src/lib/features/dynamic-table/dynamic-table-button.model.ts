import { GWCommon } from "../../common";

export class DynamicTableButton {
  public id: string;
  public name: string;
  public class: string;
  public text: string;
  public visible: boolean;

  constructor(id: string, name: string, className: string = 'btn btn-primary', text: string = 'Add', visible: boolean = false) {
    this.id = id;
    this.name = name;
    this.class = className;
    this.text = text;
    this.visible = visible;
  }
}
