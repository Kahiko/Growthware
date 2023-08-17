import { IFileUploadParameters } from "./file-upload-parameters.model";

export interface IMultiPartFileUploadParameters extends IFileUploadParameters {
  action: string;
  totalNumberOfUploads: number;
  uploadNumber: number;
  retryNumber: number;
  startingByte: number;
  endingByte: number;
}

export class MultiPartFileUploadParameters implements IMultiPartFileUploadParameters {
  public endingByte: number = 0;
  public retryNumber: number = 0;
  public startingByte: number = 0;
  public uploadNumber: number = 0;

  constructor(
    public action: string,
    public file: File,
    public totalNumberOfUploads: number,
    public chunkSize: number,
  ) {}
}
