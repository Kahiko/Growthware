import { TestBed } from '@angular/core/testing';

import { StatesService } from './states.service';

describe('StatesService', () => {
  let service: StatesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StatesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
