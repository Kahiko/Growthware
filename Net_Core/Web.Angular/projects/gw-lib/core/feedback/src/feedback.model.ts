export interface IFeedback {
    feedbackId: number;
    areaFound: string;
    assigneeId: number;
    assignee: string;
    dateClosed: string;
    dateOpened: string;
    details: string;
    foundInVersion: string;
    notes: string;
    severity: string;
    status: string;
    submittedBy: string;
    targetVersion: string;
    type: string;
    verifiedById: number;
    verifiedBy: string;
    start_Date: string;
    end_Date: string;
}

export class Feedback implements IFeedback {
    feedbackId = -1;
    areaFound = '';
    assigneeId = 1;
    assignee = 'Anonymous';
    dateClosed = '';
    dateOpened = new Date().toString();
    details = '';
    foundInVersion = '';
    notes = '';
    severity = '';
    status = '';
    submittedBy = '';
    targetVersion = '';
    type = '';
    verifiedById = 1;
    verifiedBy = '';
    start_Date = new Date().toString();
    end_Date = '';

    constructor (areaFound: string, details: string) {
        this.areaFound = areaFound;
        this.details = details;
    }
}