import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchFeedbacksComponent } from './search-feedbacks.component';

describe('SearchFeedbacksComponent', () => {
  let component: SearchFeedbacksComponent;
  let fixture: ComponentFixture<SearchFeedbacksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchFeedbacksComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchFeedbacksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
