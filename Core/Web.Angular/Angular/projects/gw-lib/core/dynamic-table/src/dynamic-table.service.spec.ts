import { TestBed } from '@angular/core/testing';

import { DynamicTableService } from './dynamic-table.service';

describe('DynamicTableService', () => {
	let service: DynamicTableService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(DynamicTableService);
	});

	it('(not implemented yet) should be created', () => {
		expect(service).toBeTruthy();
	});
});
