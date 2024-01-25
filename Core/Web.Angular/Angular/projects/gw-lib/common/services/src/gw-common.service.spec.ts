import { TestBed } from '@angular/core/testing';

import { GwCommonService } from './gw-common.service';

describe('GwCommonService', () => {
	let service: GwCommonService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(GwCommonService);
	});

	it('(not implemented yet) should be created', () => {
		expect(service).toBeTruthy();
	});
});
