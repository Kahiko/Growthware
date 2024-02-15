import { ITotalRecords } from '@growthware/common/interfaces';

export abstract class BaseService {
	abstract addEditModalId: string;
	abstract modalReason: string;
	abstract selectedRow: ITotalRecords;
}
