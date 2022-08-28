import { TestBed } from '@angular/core/testing';

import { MenuListService } from './menu-list.service';

describe('MenuListService', () => {
  let service: MenuListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MenuListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
