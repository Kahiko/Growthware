import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableFileListComponent } from './table-file-list.component';

describe('TableFileListComponent', () => {
  let component: TableFileListComponent;
  let fixture: ComponentFixture<TableFileListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableFileListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TableFileListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
