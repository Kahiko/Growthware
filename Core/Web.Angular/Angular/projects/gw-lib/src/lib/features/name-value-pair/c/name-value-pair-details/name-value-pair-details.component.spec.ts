import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NameValuePairDetailsComponent } from './name-value-pair-details.component';

describe('NameValuePairDetailsComponent', () => {
  let component: NameValuePairDetailsComponent;
  let fixture: ComponentFixture<NameValuePairDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NameValuePairDetailsComponent]
    });
    fixture = TestBed.createComponent(NameValuePairDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
