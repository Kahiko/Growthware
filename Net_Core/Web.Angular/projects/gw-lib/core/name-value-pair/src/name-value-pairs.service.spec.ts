import { TestBed } from '@angular/core/testing';

import { NameValuePairService } from './name-value-pairs.service';

describe('NameValuePairService', () => {
	let service: NameValuePairService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(NameValuePairService);
	});

	it('(not yet implemented) should be created', () => {
		expect(service).toBeTruthy();
	});
});
