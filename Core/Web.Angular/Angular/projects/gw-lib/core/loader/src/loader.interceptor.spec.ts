import { TestBed } from '@angular/core/testing';
import { HttpInterceptorFn } from '@angular/common/http';

import { LoaderInterceptor } from './loader.interceptor';

describe('loaderInterceptor', () => {
	const interceptor: HttpInterceptorFn = (req, next) => 
		TestBed.runInInjectionContext(() => LoaderInterceptor(req, next));

	beforeEach(() => {
		TestBed.configureTestingModule({});
	});

	it('(not yet implemented) should be created', () => {
		expect(interceptor).toBeTruthy();
	});
});
