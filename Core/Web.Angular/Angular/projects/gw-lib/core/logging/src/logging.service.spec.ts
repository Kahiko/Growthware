import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';

import { LoggingService } from './logging.service';

describe('LoggingService', () => {
	let service: LoggingService;

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
		service = TestBed.inject(LoggingService);
	});

	it('(not implemented yet) should be created', () => {
		expect(service).toBeTruthy();
		expect(true).toBeTrue();
	});
});
