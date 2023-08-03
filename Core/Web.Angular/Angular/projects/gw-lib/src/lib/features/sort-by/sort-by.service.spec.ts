import { TestBed } from '@angular/core/testing';

import { SortByService } from './sort-by.service';

describe('SortByService', () => {
  let service: SortByService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SortByService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
