import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchrolesComponent } from './searchroles.component';

describe('SearchrolesComponent', () => {
  let component: SearchrolesComponent;
  let fixture: ComponentFixture<SearchrolesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchrolesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchrolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
