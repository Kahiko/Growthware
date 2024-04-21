export interface IDBInformation {
    enableInheritance: number;
    informationSeqId: number;
    version: string;
}

export class DBInformation implements IDBInformation {
	public enableInheritance: number = -1;
	public informationSeqId: number = -1;
	public version: string = '';

	constructor() {}
}
