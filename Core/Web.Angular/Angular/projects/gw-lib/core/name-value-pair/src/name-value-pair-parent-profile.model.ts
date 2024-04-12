export interface INvpParentProfile {
    NVPSeqId: number;
    Schema_Name: string;
    Static_Name: string;
    Display: string;
    Description: string;
    status: number;
}

export class NvpParentProfile implements INvpParentProfile {
	NVPSeqId = -1;
	Schema_Name = '';
	Static_Name = '';
	Display = '';
	Description = '';
	status = -1;
}
