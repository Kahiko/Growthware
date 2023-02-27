import { IFileUploadParameters } from "./file-upload-parameters.model";

export interface IMultiPartFileUploadParameters extends IFileUploadParameters {
  endingByte: number;
  id: string;
  retryNumber: number;
  startingByte: number;
  totalNumberOfUploads: number;
  uploadNumber: number;
}

export class MultiPartFileUploadParameters implements IMultiPartFileUploadParameters {
  constructor(
    public id: string,
    public file: File,
    public totalNumberOfUploads: number,
    public uploadNumber: number = 0,
    public retryNumber: number = 0,
    public startingByte: number = 0,
    public endingByte: number = 29000000,
    public chunkSize: number = 29000000,
    public selectedPath: string = ''
  ) {}
}
