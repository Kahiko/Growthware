import { ITotalRecords } from '@growthware/common/interfaces';

export interface ISelectedRow extends ITotalRecords {
    FeedbackId: number;
    AssigneeId: number;
    Assignee: string;
    Details: string;
    FoundInVersion: string;
    Notes: string;
    Severity: string;
    Status: string;
    TargetVersion: string;
    Type: string;
    VerifiedById: number;
    VerifiedBy: string;
    Start_Date: string;
    TotalRecords: number;
    End_Date: string;
}

export class SelectedRow implements ISelectedRow {
    FeedbackId: number = -1;
    AssigneeId: number = -1;
    Assignee: string = '';
    Details: string = '';
    FoundInVersion: string = '';
    Notes: string = '';
    Severity: string = '';
    Status: string = '';
    TargetVersion: string = '';
    Type: string = '';
    VerifiedById: number = -1;
    VerifiedBy: string = '';
    Start_Date: string = '';
    TotalRecords: number = -1;
    End_Date: string = '';
}
