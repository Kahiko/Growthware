import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDbInformationComponent } from './edit-db-information.component';

describe('EditDbInformationComponent', () => {
  let component: EditDbInformationComponent;
  let fixture: ComponentFixture<EditDbInformationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditDbInformationComponent]
    });
    fixture = TestBed.createComponent(EditDbInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
