import { TestBed } from '@angular/core/testing';

import { MessageService } from './message.service';

describe('MessageService', () => {
	let service: MessageService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(MessageService);
	});

	it('(not yet implemented) should be created', () => {
		expect(service).toBeTruthy();
	});
});
