export interface IDirectoryData {
    directory: string;
    id: number;
    impersonate: boolean;
    impersonateAccount: string;
    impersonatePassword: string;
}

export class DirectoryData implements IDirectoryData {
    public directory: string = '';
    public id: number = -1;
    public impersonate: boolean = false;
    public impersonateAccount: string = '';
    public impersonatePassword: string = '';

    constructor() {}
}
