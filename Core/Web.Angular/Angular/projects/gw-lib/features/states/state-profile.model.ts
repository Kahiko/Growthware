export interface IStateProfile {
    description: string;
    state: string;
    status: number;
}

export class StateProfile implements IStateProfile {
    public description = '';
    public state = '';
    public status = -1;
}