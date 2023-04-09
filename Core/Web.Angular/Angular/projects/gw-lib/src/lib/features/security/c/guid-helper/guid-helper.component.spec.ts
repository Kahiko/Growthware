import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuidHelperComponent } from './guid-helper.component';

describe('GuidHelperComponent', () => {
  let component: GuidHelperComponent;
  let fixture: ComponentFixture<GuidHelperComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GuidHelperComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuidHelperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
