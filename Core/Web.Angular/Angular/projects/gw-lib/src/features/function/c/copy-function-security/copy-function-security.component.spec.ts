import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CopyFunctionSecurityComponent } from './copy-function-security.component';

describe('CopyFunctionSecurityComponent', () => {
  let component: CopyFunctionSecurityComponent;
  let fixture: ComponentFixture<CopyFunctionSecurityComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CopyFunctionSecurityComponent]
    });
    fixture = TestBed.createComponent(CopyFunctionSecurityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
