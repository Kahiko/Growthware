import { TestBed } from '@angular/core/testing';

import { NameValuePairsService } from './name-value-pairs.service';

describe('NameValuePairsService', () => {
  let service: NameValuePairsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NameValuePairsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
