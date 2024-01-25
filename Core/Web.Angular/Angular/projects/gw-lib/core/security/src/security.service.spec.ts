import { TestBed } from '@angular/core/testing';

import { SecurityService } from './security.service';

describe('SecurityService', () => {
	let service: SecurityService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(SecurityService);
	});

	it('(not yet implemented) should be created', () => {
		expect(service).toBeTruthy();
	});
});
