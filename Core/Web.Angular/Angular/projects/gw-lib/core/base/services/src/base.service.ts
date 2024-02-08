import { ISelectedRow } from '@growthware/core/base/interfaces';

export abstract class BaseService {
	abstract addEditModalId: string;
	abstract modalReason: string;
	abstract selectedRow: ISelectedRow;
}
