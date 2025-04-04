import { TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';

import { SecurityEntityService } from './security-entity.service';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('SecurityEntityService', () => {
	let service: SecurityEntityService;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
    declarations: [],
    imports: [RouterTestingModule.withRoutes([]),
        NoopAnimationsModule],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
}).compileComponents();
		service = TestBed.inject(SecurityEntityService);
	});

	it('(not yet implemented) should be created', () => {
		expect(service).toBeTruthy();
	});
});
