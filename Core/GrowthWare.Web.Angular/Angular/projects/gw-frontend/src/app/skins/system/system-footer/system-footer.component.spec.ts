import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemFooterComponent } from './system-footer.component';

describe('SystemFooterComponent', () => {
  let component: SystemFooterComponent;
  let fixture: ComponentFixture<SystemFooterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SystemFooterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
