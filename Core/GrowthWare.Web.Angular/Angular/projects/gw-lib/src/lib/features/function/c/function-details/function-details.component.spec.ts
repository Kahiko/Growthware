import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FunctionDetailsComponent } from './function-details.component';

describe('FunctionDetailsComponent', () => {
  let component: FunctionDetailsComponent;
  let fixture: ComponentFixture<FunctionDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FunctionDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FunctionDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
