import { ITotalRecords } from '@growthware/common/interfaces';

export class SelectedRow implements ITotalRecords {
    Account: string = '';
    Component: string = '';
    ClassName: string = '';
    Level: string = '';
    LogSeqId: string = '';
    MethodName: string = '';
    Msg: string = '';
    TotalRecords: number = -1;
}