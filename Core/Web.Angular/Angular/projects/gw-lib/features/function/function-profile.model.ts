export interface IFunctionProfile {
  name: string;
}

export class FunctionProfile implements IFunctionProfile {
  public name: string = '';
  constructor() {}
}
