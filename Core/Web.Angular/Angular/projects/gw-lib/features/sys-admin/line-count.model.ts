export interface ILineCount {
    theDirectory: string;
    excludePattern: string;
    includeFiles: string;
}

export class LineCount implements ILineCount {
    public theDirectory: string = '';
    public excludePattern: string = ''
    public includeFiles: string = '*.json, *.cs, *.ts';

    constructor(
        theDirectory = '',
        excludePattern = '',
        includeFiles = '*.json, *.cs, *.ts'
    ) {
    }
}
