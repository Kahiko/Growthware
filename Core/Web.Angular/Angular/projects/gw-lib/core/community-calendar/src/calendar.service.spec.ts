import { TestBed } from '@angular/core/testing';

import { CalendarService } from './calendar.service';

describe('CalendarService', () => {
	let service: CalendarService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(CalendarService);
	});

	it('(not yet implemented) should be created', () => {
		expect(service).toBeTruthy();
	});
});
