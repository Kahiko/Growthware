import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StateDetailsComponent } from './state-details.component';

describe('StateDetailsComponent', () => {
  let component: StateDetailsComponent;
  let fixture: ComponentFixture<StateDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StateDetailsComponent]
    });
    fixture = TestBed.createComponent(StateDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
