import { TestBed } from '@angular/core/testing';

import { GWLibService } from '../gw-lib.service';

describe('GWLibService', () => {
  let service: GWLibService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GWLibService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
