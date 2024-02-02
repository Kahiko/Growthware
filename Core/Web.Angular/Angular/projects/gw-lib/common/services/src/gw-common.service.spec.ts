import { TestBed } from '@angular/core/testing';

import { GWCommon } from './gw-common.service';

describe('GWCommon', () => {
	let service: GWCommon;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(GWCommon);
	});

	it('(not implemented yet) should be created', () => {
		expect(service).toBeTruthy();
	});
});
