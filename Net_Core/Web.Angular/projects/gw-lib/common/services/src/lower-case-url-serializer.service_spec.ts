import { TestBed } from '@angular/core/testing';

import { LowerCaseUrlSerializer } from './lower-case-url-serializer.service';

describe('LowerCaseUrlSerializer', () => {
	/**
	 * I'm unsure that a unit test is needed for this functionality.  
	 * The test is simply paste an URL and it should be changed lower case
	 * This was tested during integration testing and is not likely to chnage without the same test.
	 */
	let service: LowerCaseUrlSerializer;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(LowerCaseUrlSerializer);
	});

	it('(not implemented yet) should be created', () => {
		expect(service).toBeTruthy();
	});
});
