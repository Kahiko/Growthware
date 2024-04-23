export interface IDynamicTableBtnMethods {
  btnTopLeftCallBackMethod: (arg?: any) => void;
  btnTopRightCallBackMethod: (arg?: any) => void;
  btnBottomLeftCallBackMethod: (arg?: any) => void;
  btnBottomRightCallBackMethod: (arg?: any) => void;
}

export class DynamicTableBtnMethods implements IDynamicTableBtnMethods {
	btnTopLeftCallBackMethod!: (arg?: any) => void;
	btnTopRightCallBackMethod!: (arg?: any) => void;
	btnBottomLeftCallBackMethod!: (arg?: any) => void;
	btnBottomRightCallBackMethod!: (arg?: any) => void;

	constructor() {}
}
