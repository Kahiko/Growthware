import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DBLogDetailsComponent } from './db-log-details.component';

describe('DBLogDetailsComponent', () => {
  let component: DBLogDetailsComponent;
  let fixture: ComponentFixture<DBLogDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DBLogDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DBLogDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
