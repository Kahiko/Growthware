import { TestBed } from '@angular/core/testing';
import { HttpInterceptorFn } from '@angular/common/http';

import { JwtInterceptor } from './jwt.interceptor';

describe('jWTInterceptor', () => {
	const interceptor: HttpInterceptorFn = (req, next) => 
		TestBed.runInInjectionContext(() => JwtInterceptor(req, next));

	beforeEach(() => {
		TestBed.configureTestingModule({});
	});

	it('(not yet implemented) should be created', () => {
		expect(interceptor).toBeTruthy();
	});
});
