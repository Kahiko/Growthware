import { CallbackMethod } from './gw.types';

export interface ICallbackButton {
  "id": string,
  "name": string,
  "class": string,
  "color": string,
  "text": string,
  "visible": boolean,
  "callbackMethod"?: CallbackMethod,
}

export class CallbackButton implements ICallbackButton {
  public id: string;
  public name: string;
  public class: string;
  public color: string;
  public text: string;
  public visible: boolean;
  public callbackMethod?: CallbackMethod;

  /**
   * Creates an instance of CallbackButton.
   * @param {string} text
   * @param {string} id
   * @param {string} name
   * @param {boolean} [visible=false]
   * @param {string} [color='primary']
   * @param {string} [className='btn btn-primary']
   * @memberof CallbackButton
   */
  constructor(
      text: string,
      id: string,
      name:
      string,
      visible: boolean = false,
      color: string = 'primary',
      className: string = 'btn btn-primary'
    ) {
    this.id = id;
    this.name = name;
    this.class = className;
    this.color = color;
    this.text = text;
    this.visible = visible;
  }
}
