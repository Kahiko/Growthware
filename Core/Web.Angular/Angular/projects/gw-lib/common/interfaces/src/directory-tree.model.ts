export interface IDirectoryTree {
    children: IDirectoryTree[];
    directoryCount: number;
    fileCount: number;
    isFolder: boolean;
    key: string;
    name: string;
    relitivePath: string;
    size: string;
    sizeWithChildren: string;
    sizeInBytes: number;
    sizeInBytesWithChildren: number;
}

export class DirectoryTree implements IDirectoryTree {
	public children: IDirectoryTree[] = [];
	public directoryCount: number = 0;
	public fileCount: number = 0;
	public isFolder: boolean = true;
	public key: string = '';
	public name: string = '';
	public relitivePath: string = '';
	public size: string = '';
	public sizeWithChildren: string = '';
	public sizeInBytes: number = 0;
	public sizeInBytesWithChildren: number = 0;
}

