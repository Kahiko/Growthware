export interface IFeedback {
    feedbackId: number;
    action: string;
    assignee: string;
    assigneeId: number;
    dateClosed: string;
    dateOpened: string;
    details: string;
    foundInVersion: string;
    functionSeqId: number;
    notes: string;
    severity: string;
    status: string;
    submittedBy: string;
    submittedById: number;
    targetVersion: string;
    type: string;
    updatedBy: string;
    updatedById: number;
    verifiedBy: string;
    verifiedById: number;
}

export class Feedback implements IFeedback {
    feedbackId = -1;
    action = '';
    areaFound = '';
    assigneeId = 1;
    assignee = 'Anonymous';
    dateClosed = '';
    dateOpened = new Date().toString();
    details = '';
    foundInVersion = '';
    functionSeqId = -1;
    notes = '';
    severity = '';
    status = '';
    submittedBy = '';
    submittedById = -1;
    targetVersion = '';
    type = '';
    updatedBy = '';
    updatedById = -1;
    verifiedBy = '';
    verifiedById = 1;
    constructor (areaFound: string, details: string) {
        this.areaFound = areaFound;
        this.details = details;
    }
}