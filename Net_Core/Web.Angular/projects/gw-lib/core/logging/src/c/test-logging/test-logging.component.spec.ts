import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestLoggingComponent } from './test-logging.component';

describe('TestLoggingComponent', () => {
  let component: TestLoggingComponent;
  let fixture: ComponentFixture<TestLoggingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TestLoggingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestLoggingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
