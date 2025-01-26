export interface IFormDataParameters {
  action: string;                     // The action the upload is for this is unique to Growthware and is part of the security
  doMerge: boolean;                   // If true, the API runs the merge logic
  formFile: Blob | null | undefined;  // The file or slice of a file
  fileId: string;                     // A "unique" ID for a given file based on its name, size, and modified date
  fileName: string;                   // The original name of the file being uploaded
  uploadIndex: number;                // The index of the upload
  totalUploads: number;               // The total number of uploads for the file
}

export class FormDataParameters implements IFormDataParameters {
  constructor(
    public action: string,
    public doMerge: boolean,
    public formFile: Blob | null | undefined,
    public fileId: string,
    public fileName: string,
    public uploadIndex: number,
    public totalUploads: number
  ) { }
}
