export interface INvpChildProfile {
    id: number;
    nameValuePairSeqId: number;
    status: number;
    sortOrder: number;
    text: string;
    value: string;
    addedBy: number;
    AddedDate: string;
    updatedBy: number;
    updatedDate: string;
}

export class NvpChildProfile implements INvpChildProfile {
	id: number = -1;
	nameValuePairSeqId: number = -1;
	status: number = 1;
	sortOrder: number = 0;
	text: string = '';
	value: string = '';
	addedBy: number = -1;
	AddedDate: string = '';
	updatedBy: number = -1;
	updatedDate: string = '';
}
