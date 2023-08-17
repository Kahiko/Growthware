import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LineCountComponent } from './line-count.component';

describe('LineCountComponent', () => {
  let component: LineCountComponent;
  let fixture: ComponentFixture<LineCountComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LineCountComponent]
    });
    fixture = TestBed.createComponent(LineCountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
