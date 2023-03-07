import { TestBed } from '@angular/core/testing';

import { GWCommon } from './gwcommon.service';

describe('GWCommon', () => {
  let service: GWCommon;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GWCommon);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
