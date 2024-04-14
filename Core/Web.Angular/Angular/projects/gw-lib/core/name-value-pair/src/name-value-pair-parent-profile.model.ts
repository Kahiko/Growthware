export interface INvpParentProfile {
    nvpSeqId: number;
    schemaName: string;
    staticName: string;
    display: string;
    description: string;
    status: number;
}

export class NvpParentProfile implements INvpParentProfile {
	nvpSeqId = -1;
	schemaName = '';
	staticName = '';
	display = '';
	description = '';
	status = -1;
}
