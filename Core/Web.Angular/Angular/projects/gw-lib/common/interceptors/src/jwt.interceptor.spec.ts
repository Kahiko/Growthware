import { TestBed } from '@angular/core/testing';
import { HttpInterceptorFn } from '@angular/common/http';

import { jWTInterceptor } from './jwt.interceptor';

describe('jWTInterceptor', () => {
  const interceptor: HttpInterceptorFn = (req, next) => 
    TestBed.runInInjectionContext(() => jWTInterceptor(req, next));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('(not yet implemented) should be created', () => {
    expect(interceptor).toBeTruthy();
  });
});
