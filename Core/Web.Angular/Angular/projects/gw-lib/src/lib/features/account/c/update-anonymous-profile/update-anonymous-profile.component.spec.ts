import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateAnonymousProfileComponent } from './update-anonymous-profile.component';

describe('UpdateAnonymousProfileComponent', () => {
  let component: UpdateAnonymousProfileComponent;
  let fixture: ComponentFixture<UpdateAnonymousProfileComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UpdateAnonymousProfileComponent]
    });
    fixture = TestBed.createComponent(UpdateAnonymousProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
