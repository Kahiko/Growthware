import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchNameValuePairDetailsComponent } from './search-name-value-pair-details.component';

describe('SearchNameValuePairDetailsComponent', () => {
  let component: SearchNameValuePairDetailsComponent;
  let fixture: ComponentFixture<SearchNameValuePairDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SearchNameValuePairDetailsComponent]
    });
    fixture = TestBed.createComponent(SearchNameValuePairDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
