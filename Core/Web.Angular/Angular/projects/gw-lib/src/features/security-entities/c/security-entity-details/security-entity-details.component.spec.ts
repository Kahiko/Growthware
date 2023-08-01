import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecurityEntityDetailsComponent } from './security-entity-details.component';

describe('SecurityEntityDetailsComponent', () => {
  let component: SecurityEntityDetailsComponent;
  let fixture: ComponentFixture<SecurityEntityDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SecurityEntityDetailsComponent]
    });
    fixture = TestBed.createComponent(SecurityEntityDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
