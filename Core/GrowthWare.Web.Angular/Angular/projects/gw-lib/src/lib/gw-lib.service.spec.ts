import { TestBed } from '@angular/core/testing';

import { GwLibService } from './gw-lib.service';

describe('GwLibService', () => {
  let service: GwLibService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GwLibService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
