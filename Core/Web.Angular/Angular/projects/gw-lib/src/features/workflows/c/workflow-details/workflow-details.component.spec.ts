import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowDetailsComponent } from './workflow-details.component';

describe('WorkflowDetailsComponent', () => {
  let component: WorkflowDetailsComponent;
  let fixture: ComponentFixture<WorkflowDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WorkflowDetailsComponent]
    });
    fixture = TestBed.createComponent(WorkflowDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
