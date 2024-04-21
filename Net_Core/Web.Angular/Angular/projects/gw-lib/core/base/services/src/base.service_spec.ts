import { TestBed } from '@angular/core/testing';

import { BaseService } from './base.service';

/**
 * There really isn't anything to test it is more or less a interface
 */
describe('BaseService', () => {
	let service: BaseService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(BaseService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
