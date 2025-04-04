export interface IFileInfoLight {
	created: Date;
	createdShort: string;
	extension: string;
	fullName: string;
	modified: Date;
	modifiedShort: string
	name: string;
	selected: boolean;
	shortFileName: string;
	size: string;
	visible: boolean;
}

export class FileInfoLight implements IFileInfoLight {
	created: Date = new Date();
	createdShort: string = '';
	extension: string = '';
	fullName: string = '';
	modified: Date = new Date();
	modifiedShort: string = '';
	name: string = '';
	selected: boolean = false;
	shortFileName: string = '';
	size: string = '';
	visible: boolean = false;

	constructor(
		created: Date,
		createdShort: string,
		extension: string,
		fullName: string,
		modified: Date,
		modifiedShort: string,
		name: string,
		shortFileName: string,
		size: string,
	) {
		this.created = created;
		this.createdShort = createdShort;
		this.extension = extension;
		this.fullName = fullName;
		this.modified = modified;
		this.modifiedShort = modifiedShort;
		this.name = name;
		this.shortFileName = shortFileName;
		this.size = size;
	}
}
