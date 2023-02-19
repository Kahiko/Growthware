export interface IDirectoryTree {
    children: IDirectoryTree[];
    directoryCount: number;
    fileCount: number;
    isFolder: boolean;
    key: string;
    name: string;
}

export class DirectoryTree implements IDirectoryTree {
    public children: IDirectoryTree[] = [];
    public directoryCount: number = 0;
    public fileCount: number = 0;
    public isFolder: boolean = true;
    public key: string = '';
    public name: string = '';
}
