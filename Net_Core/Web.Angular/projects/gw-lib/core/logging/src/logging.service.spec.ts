import { TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';

import { LoggingService } from './logging.service';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('LoggingService', () => {
	let service: LoggingService;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [RouterTestingModule,
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
		service = TestBed.inject(LoggingService);
	});

	it('(not implemented yet) should be created', () => {
		expect(service).toBeTruthy();
		expect(true).toBeTrue();
	});
});
