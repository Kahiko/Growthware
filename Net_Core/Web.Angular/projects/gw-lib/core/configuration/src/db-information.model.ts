export interface IDBInformation {
    databaseInformationSeqId: number;
    enableInheritance: number;
    version: string;
}

export class DBInformation implements IDBInformation {
	public databaseInformationSeqId: number = -1;
	public enableInheritance: number = -1;
	public version: string = '';

	constructor() {}
}
