export interface INvpParentProfile {
    NVPSeqId: number;
    Schema_Name: string;
    Static_Name: string;
    Display: string;
    Description: string;
    StatusSeqId: number;
}

export class NvpParentProfile implements INvpParentProfile {
	NVPSeqId = -1;
	Schema_Name = '';
	Static_Name = '';
	Display = '';
	Description = '';
	StatusSeqId = -1;
}
