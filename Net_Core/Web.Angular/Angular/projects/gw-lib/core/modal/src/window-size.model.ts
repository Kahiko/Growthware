export interface IWindowSize {
  'pxHeight': number;
  'pxWidth': number;
}

export class WindowSize implements IWindowSize {

	constructor(
    public pxHeight: number,
    public pxWidth: number,
	) {}
}
