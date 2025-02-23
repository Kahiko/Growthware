export interface IFeedback {
    action: string;
    assignee: string;
    assigneeId: number;
    dateClosed: string;
    dateOpened: string;
    details: string;
    feedbackId: number;
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
	assigneeId = 1;
	assignee = 'Anonymous';
	dateClosed = '1753-01-01T00:00:00';
	dateOpened = '1753-01-01T00:00:00';
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
		this.action = areaFound;
		this.details = details;
	}
}