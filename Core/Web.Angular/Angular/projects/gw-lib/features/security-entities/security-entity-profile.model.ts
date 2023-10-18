export interface ISecurityEntityProfile {
    connectionString: string;
    description: string;
    dataAccessLayer: string;
    dataAccessLayerAssemblyName: string;
    dataAccessLayerNamespace: string;
    encryptionType: number;
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
    parentSeqId = -1;
    skin = '';
    style = '';
    statusSeqId = -1;
    url = '';
}
