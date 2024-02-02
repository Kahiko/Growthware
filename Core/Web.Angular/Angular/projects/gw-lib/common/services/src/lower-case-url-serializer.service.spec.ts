import { TestBed } from '@angular/core/testing';

import { LowerCaseUrlSerializer } from './lower-case-url-serializer.service';

describe('LowerCaseUrlSerializer', () => {
	let service: LowerCaseUrlSerializer;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(LowerCaseUrlSerializer);
	});

	it('(not implemented yet) should be created', () => {
		expect(service).toBeTruthy();
	});
});
