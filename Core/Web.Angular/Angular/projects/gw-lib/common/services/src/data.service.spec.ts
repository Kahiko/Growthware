import { TestBed } from '@angular/core/testing';

import { DataService } from './data.service';

describe('DataService', () => {
	let service: DataService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(DataService);
	});

	it('(not implemented yet) should be created', () => {
		expect(service).toBeTruthy();
	});
});
