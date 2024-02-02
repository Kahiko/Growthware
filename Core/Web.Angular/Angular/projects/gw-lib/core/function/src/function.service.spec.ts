import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';

import { FunctionService } from './function.service';

describe('FunctionService', () => {
	let service: FunctionService;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				RouterTestingModule,
				HttpClientTestingModule,
				NoopAnimationsModule,
			],
			declarations: [],
			providers: [ ]
		}).compileComponents();
		service = TestBed.inject(FunctionService);
	});

	it('(not yet implemented) should be created', () => {
		expect(service).toBeTruthy();
	});
});
