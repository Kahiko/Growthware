import { TestBed } from '@angular/core/testing';

import { ClientChoicesService } from './client-choices.service';

describe('ClientChoicesService', () => {
  let service: ClientChoicesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientChoicesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
