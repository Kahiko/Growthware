import { TestBed } from '@angular/core/testing';

import { SecurityEntityService } from './security-entity.service';

describe('SecurityEntityService', () => {
  let service: SecurityEntityService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SecurityEntityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
