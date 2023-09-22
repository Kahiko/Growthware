export interface IMessageProfile {
    body: string;
    description: string;
    id: number;
    title: string;
    securityEntitySeqId: number;
    formatAsHtml: boolean;
}

export class MessageProfile {
    body: string = '';
    description: string = '';
    id: number = -1;
    title: string = '';
    securityEntitySeqId: number = 1;
    formatAsHtml: boolean = true;
}