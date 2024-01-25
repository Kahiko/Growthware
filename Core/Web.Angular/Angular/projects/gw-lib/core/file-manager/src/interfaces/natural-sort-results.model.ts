
export interface IMColumns {
    col1: string;
    col2: string;
}

export interface INaturalSortResults {
    dataTable: IMColumns[];
    dataView: IMColumns[];
    startTime: string;
    stopTime: string;
    totalMilliseconds: string;
}
