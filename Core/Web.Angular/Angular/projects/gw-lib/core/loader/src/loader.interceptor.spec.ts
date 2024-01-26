import { TestBed } from '@angular/core/testing';
import { HttpInterceptorFn } from '@angular/common/http';

import { loaderInterceptor } from './loader.interceptor';

describe('loaderInterceptor', () => {
	const interceptor: HttpInterceptorFn = (req, next) => 
		TestBed.runInInjectionContext(() => loaderInterceptor(req, next));

	beforeEach(() => {
		TestBed.configureTestingModule({});
	});

	it('(not yet implemented) should be created', () => {
		expect(interceptor).toBeTruthy();
	});
});
