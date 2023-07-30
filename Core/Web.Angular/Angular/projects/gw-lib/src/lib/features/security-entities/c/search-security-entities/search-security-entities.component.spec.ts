import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchSecurityEntitiesComponent } from './search-security-entities.component';

describe('SearchSecurityEntitiesComponent', () => {
  let component: SearchSecurityEntitiesComponent;
  let fixture: ComponentFixture<SearchSecurityEntitiesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SearchSecurityEntitiesComponent]
    });
    fixture = TestBed.createComponent(SearchSecurityEntitiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
