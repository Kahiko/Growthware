import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchDBLogsComponent } from './search-db-logs.component';

describe('SearchDBLogsComponent', () => {
  let component: SearchDBLogsComponent;
  let fixture: ComponentFixture<SearchDBLogsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchDBLogsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchDBLogsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
