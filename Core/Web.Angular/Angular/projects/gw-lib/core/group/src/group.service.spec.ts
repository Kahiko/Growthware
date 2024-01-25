import { TestBed } from '@angular/core/testing';

import { GroupService } from './group.service';

describe('GroupService', () => {
	let service: GroupService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(GroupService);
	});

	it('(not yet implemented) should be created', () => {
		expect(service).toBeTruthy();
	});
});
