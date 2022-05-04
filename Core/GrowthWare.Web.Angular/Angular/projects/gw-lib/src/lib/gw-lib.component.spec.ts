import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GwLibComponent } from './gw-lib.component';

describe('GwLibComponent', () => {
  let component: GwLibComponent;
  let fixture: ComponentFixture<GwLibComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GwLibComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GwLibComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
