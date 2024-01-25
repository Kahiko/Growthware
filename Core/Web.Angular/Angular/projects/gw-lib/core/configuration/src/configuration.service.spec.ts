import { TestBed } from '@angular/core/testing';

import { ConfigurationService } from './configuration.service';

describe('ConfigurationService', () => {
	let service: ConfigurationService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(ConfigurationService);
	});

	it('(not yet implemented) should be created', () => {
		expect(service).toBeTruthy();
	});
});
