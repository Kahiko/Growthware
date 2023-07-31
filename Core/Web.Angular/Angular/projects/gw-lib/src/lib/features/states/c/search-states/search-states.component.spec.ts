import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchStatesComponent } from './search-states.component';

describe('SearchStatesComponent', () => {
  let component: SearchStatesComponent;
  let fixture: ComponentFixture<SearchStatesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SearchStatesComponent]
    });
    fixture = TestBed.createComponent(SearchStatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
