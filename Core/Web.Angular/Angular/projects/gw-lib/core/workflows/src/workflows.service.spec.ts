import { TestBed } from '@angular/core/testing';

import { WorkflowsService } from './workflows.service';

describe('WorkflowsService', () => {
	let service: WorkflowsService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(WorkflowsService);
	});

	it('(not yet implemented) should be created', () => {
		expect(service).toBeTruthy();
	});
});
