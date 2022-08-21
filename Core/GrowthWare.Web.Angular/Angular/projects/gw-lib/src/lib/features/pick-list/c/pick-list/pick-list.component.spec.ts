import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PickListComponent } from './pick-list.component';

describe('PickListComponent', () => {
  let component: PickListComponent;
  let fixture: ComponentFixture<PickListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PickListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PickListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
