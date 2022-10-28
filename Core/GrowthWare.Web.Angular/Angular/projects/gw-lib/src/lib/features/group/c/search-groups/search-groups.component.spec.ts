import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchGroupsComponent } from './search-groups.component';

describe('SearchgroupsComponent', () => {
  let component: SearchGroupsComponent;
  let fixture: ComponentFixture<SearchGroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchGroupsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
