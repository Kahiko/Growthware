export interface IFunctionMenuOrder {
    function_Seq_Id: number;
    action: string;
    name: string;
}

export class FunctionMenuOrder implements IFunctionMenuOrder {
    public function_Seq_Id: number = -1;
    public action: string = '';
    public name: string = '';
}
