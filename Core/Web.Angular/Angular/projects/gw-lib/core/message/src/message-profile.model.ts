export interface IMessageProfile {
    avalibleTags: string;
    body: string;
    description: string;
    formatAsHtml: boolean;
    id: number;
    name: string;
    securityEntitySeqId: number;
    title: string;
}

export class MessageProfile implements IMessageProfile {
	avalibleTags: string = '';
	body: string = '';
	description: string = '';
	formatAsHtml: boolean = true;
	id: number = -1;
	name: string = '';
	securityEntitySeqId: number = 1;
	title: string = '';
}