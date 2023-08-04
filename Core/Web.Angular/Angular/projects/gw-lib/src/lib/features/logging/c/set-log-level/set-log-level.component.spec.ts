import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetLogLevelComponent } from './set-log-level.component';

describe('SetLogLevelComponent', () => {
  let component: SetLogLevelComponent;
  let fixture: ComponentFixture<SetLogLevelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SetLogLevelComponent]
    });
    fixture = TestBed.createComponent(SetLogLevelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
