export interface IKeyValuePair {
    key: number;
    value: string;
}

export class KeyValuePair implements IKeyValuePair {
    public key: number = -1;
    public value: string = '';
}