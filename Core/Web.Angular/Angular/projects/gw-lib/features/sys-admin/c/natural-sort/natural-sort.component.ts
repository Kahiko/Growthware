import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
// Angular Material
import { MatTableModule } from '@angular/material/table';
// Library
import { FileManagerService } from '@Growthware/features/file-manager';
import { LoggingService } from '@Growthware/features/logging';

@Component({
  selector: 'gw-lib-natural-sort',
  standalone: true,
  imports: [
    CommonModule,
  
    MatTableModule
  ],
  templateUrl: './natural-sort.component.html',
  styleUrls: ['./natural-sort.component.scss']
})
export class NaturalSortComponent implements OnInit {

  // dataTableSource: any[] = [
  //   {col1: 1, col2: 'Hydrogen', weight: 1.0079, symbol: 'H'},
  //   {col1: 2, col2: 'Helium', weight: 4.0026, symbol: 'He'},
  //   {col1: 3, col2: 'Lithium', weight: 6.941, symbol: 'Li'},
  //   {col1: 4, col2: 'Beryllium', weight: 9.0122, symbol: 'Be'},
  //   {col1: 5, col2: 'Boron', weight: 10.811, symbol: 'B'},
  //   {col1: 6, col2: 'Carbon', weight: 12.0107, symbol: 'C'},
  //   {col1: 7, col2: 'Nitrogen', weight: 14.0067, symbol: 'N'},
  //   {col1: 8, col2: 'Oxygen', weight: 15.9994, symbol: 'O'},
  //   {col1: 9, col2: 'Fluorine', weight: 18.9984, symbol: 'F'},
  //   {col1: 10, col2: 'Neon', weight: 20.1797, symbol: 'Ne'},
  // ];
  dataTableSource: any[] = [];
  
  displayedColumns: string[] = ['col1', 'col2'];
  dataViewSource: any[] = [{coL1: '', coL2: ''}];

  constructor(
    private _FileManagerSvc: FileManagerService,
    private _LoggingSvc: LoggingService
  ) { }

  ngOnInit(): void {
    this._FileManagerSvc.getTestNaturalSort('ASC').then((response: any) => {
      // console.log('NaturalSortComponent.ngOnInit',response.dataTable);
      this.dataTableSource = response.dataTable;
      this.dataViewSource = response.dataView;
    }).catch((error) => {
      this._LoggingSvc.errorHandler(error, 'FileManagerService', 'getTestNaturalSort');
    })
  }

}
