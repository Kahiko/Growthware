import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchfunctionsComponent } from './searchfunctions.component';

describe('SearchfunctionsComponent', () => {
  let component: SearchfunctionsComponent;
  let fixture: ComponentFixture<SearchfunctionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchfunctionsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchfunctionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
