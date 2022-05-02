import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GWLibComponent } from './gw-lib.component';

describe('GWLibComponent', () => {
  let component: GWLibComponent;
  let fixture: ComponentFixture<GWLibComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GWLibComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GWLibComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
