import { TestBed } from '@angular/core/testing';
import { HttpInterceptorFn } from '@angular/common/http';

import { ErrorInterceptor } from './error.interceptor';

describe('errorInterceptor', () => {
	const interceptor: HttpInterceptorFn = (req, next) =>
		TestBed.runInInjectionContext(() => ErrorInterceptor(req, next));

	beforeEach(() => {
		TestBed.configureTestingModule({});
	});

	it('(not yet implemented) should be created', () => {
		expect(interceptor).toBeTruthy();
	});
});
