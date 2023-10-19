export interface ISecurityEntityProfile {
    connectionString: string;
    description: string;
    dataAccessLayer: string;
    dataAccessLayerAssemblyName: string;
    dataAccessLayerNamespace: string;
    encryptionType: number;
    name: string;
    parentSeqId: number;
    skin: string;
    style: string;
    statusSeqId: number;
    url: string;
}

export class SecurityEntityProfile implements ISecurityEntityProfile {
    connectionString = '';
    description = '';
    dataAccessLayer = '';
    dataAccessLayerAssemblyName = '';
    dataAccessLayerNamespace = '';
    encryptionType = -1;
    name = '';
    parentSeqId = -1;
    skin = '';
    style = '';
    statusSeqId = -1;
    url = '';
}
