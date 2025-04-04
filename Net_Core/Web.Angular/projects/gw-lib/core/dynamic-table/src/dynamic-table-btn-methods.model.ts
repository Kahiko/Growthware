export interface IDynamicTableBtnMethods {
	btnTopLeftCallBackMethod: (arg?: unknown) => void;
	btnTopRightCallBackMethod: (arg?: unknown) => void;
	btnBottomLeftCallBackMethod: (arg?: unknown) => void;
	btnBottomRightCallBackMethod: (arg?: unknown) => void;
}

export class DynamicTableBtnMethods implements IDynamicTableBtnMethods {
	btnTopLeftCallBackMethod!: (arg?: unknown) => void;
	btnTopRightCallBackMethod!: (arg?: unknown) => void;
	btnBottomLeftCallBackMethod!: (arg?: unknown) => void;
	btnBottomRightCallBackMethod!: (arg?: unknown) => void;

	constructor() { }
}
