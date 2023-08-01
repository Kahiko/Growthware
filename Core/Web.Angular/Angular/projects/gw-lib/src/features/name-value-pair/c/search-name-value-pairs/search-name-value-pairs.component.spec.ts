import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchNameValuePairsComponent } from './search-name-value-pairs.component';

describe('SearchNameValuePairsComponent', () => {
  let component: SearchNameValuePairsComponent;
  let fixture: ComponentFixture<SearchNameValuePairsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SearchNameValuePairsComponent]
    });
    fixture = TestBed.createComponent(SearchNameValuePairsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
