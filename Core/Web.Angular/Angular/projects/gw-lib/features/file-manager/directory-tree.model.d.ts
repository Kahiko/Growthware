export interface IDirectoryTree {
    children: IDirectoryTree[];
    directoryCount: number;
    fileCount: number;
    isFolder: boolean;
    key: string;
    name: string;
    relitivePath: string;
}
export declare class DirectoryTree implements IDirectoryTree {
    children: IDirectoryTree[];
    directoryCount: number;
    fileCount: number;
    isFolder: boolean;
    key: string;
    name: string;
    relitivePath: string;
}
