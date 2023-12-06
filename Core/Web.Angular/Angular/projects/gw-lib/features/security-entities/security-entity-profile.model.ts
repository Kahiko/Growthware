export interface ISecurityEntityProfile {
    connectionString: string;
    dataAccessLayer: string;
    dataAccessLayerAssemblyName: string;
    dataAccessLayerNamespace: string;
    description: string;
    encryptionType: number;
    id: number;
    name: string;
    parentSeqId: number;
    skin: string;
    statusSeqId: number;
    style: string;
    url: string;
}

export class SecurityEntityProfile implements ISecurityEntityProfile {
    connectionString = '';
    dataAccessLayer = 'SQLServer';
    dataAccessLayerAssemblyName = '';
    dataAccessLayerNamespace = '';
    description = '';
    encryptionType = 3;
    id = -1;
    name = '';
    parentSeqId = -1;
    skin = 'Default';
    statusSeqId = 1;
    style = 'Default';
    url = 'no url';
}
