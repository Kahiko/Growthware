import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchgroupsComponent } from './searchgroups.component';

describe('SearchgroupsComponent', () => {
  let component: SearchgroupsComponent;
  let fixture: ComponentFixture<SearchgroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchgroupsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchgroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
