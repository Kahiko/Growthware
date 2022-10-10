import { TestBed } from '@angular/core/testing';

import { LowerCaseUrlSerializerService } from './lower-case-url-serializer.service';

describe('LowerCaseUrlSerializerService', () => {
  let service: LowerCaseUrlSerializerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LowerCaseUrlSerializerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
