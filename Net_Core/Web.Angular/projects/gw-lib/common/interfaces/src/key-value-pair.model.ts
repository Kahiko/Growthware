/* eslint-disable @typescript-eslint/no-explicit-any */
export interface IKeyValuePair {
    key: number;
    value: string;
}

export class KeyValuePair implements IKeyValuePair {
	public key: number = -1;
	public value: string = '';
	constructor() { }
}

export interface IKeyDataPair {
    key: number;
    value: Array<any>;
}

export class KeyDataPair implements IKeyDataPair {

	constructor(public key: number, public value: Array<any>) { }
}

export interface INameValuePair {
    name: string;
    value: string;
}

export class NameValuePair implements INameValuePair {

	constructor(public name: string, public value: string) { }
}

export interface INameDataPair {
    name: string;
    value: Array<any>;
}

export class NameDataPair implements INameDataPair {

	constructor(public name: string, public value: Array<any>) { }
}