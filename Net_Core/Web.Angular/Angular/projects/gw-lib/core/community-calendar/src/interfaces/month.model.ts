import { IWeek } from './week.model';

export interface IMonth {
    weeks: IWeek[];
}

export class Month implements IMonth {
	public weeks: IWeek[] = [];
}
