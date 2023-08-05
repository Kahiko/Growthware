import { TestBed } from '@angular/core/testing';

import { SysAdminService } from './sys-admin.service';

describe('SysAdminService', () => {
  let service: SysAdminService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SysAdminService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
