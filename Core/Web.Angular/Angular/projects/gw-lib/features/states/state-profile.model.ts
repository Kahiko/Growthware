export interface IStateProfile {
    description: string;
    state: string;
    statusId: number;
}

export class StateProfile implements IStateProfile {
    public description = '';
    public state = '';
    public statusId = -1;
}