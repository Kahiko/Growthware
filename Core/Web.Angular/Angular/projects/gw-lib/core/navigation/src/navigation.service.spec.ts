import { TestBed } from '@angular/core/testing';

import { NavigationService } from './navigation.service';

describe('NavigationService', () => {
	let service: NavigationService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(NavigationService);
	});

	it('(not yet implemented) should be created', () => {
		expect(service).toBeTruthy();
	});
});
