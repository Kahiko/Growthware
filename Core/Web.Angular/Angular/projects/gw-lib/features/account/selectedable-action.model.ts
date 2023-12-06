export interface ISelectedableAction {
    action: string;
    title: string;
}

export class SelectedableAction implements ISelectedableAction {
    action = '';
    title = '';
}
