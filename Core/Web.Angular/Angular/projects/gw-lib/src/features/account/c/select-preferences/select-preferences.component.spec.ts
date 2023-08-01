import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectPreferencesComponent } from './select-preferences.component';

describe('SelectPreferencesComponent', () => {
  let component: SelectPreferencesComponent;
  let fixture: ComponentFixture<SelectPreferencesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SelectPreferencesComponent]
    });
    fixture = TestBed.createComponent(SelectPreferencesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
