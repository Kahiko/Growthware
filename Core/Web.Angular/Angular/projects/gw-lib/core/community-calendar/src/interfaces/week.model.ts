import { IDay } from './day.model';

export interface IWeek {
    days: IDay[]
}

export class Week implements IWeek {
	days: IDay[] = [];
}
