import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestModalComponent } from './test-modal.component';

describe('TestModalComponent', () => {
  let component: TestModalComponent;
  let fixture: ComponentFixture<TestModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TestModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
