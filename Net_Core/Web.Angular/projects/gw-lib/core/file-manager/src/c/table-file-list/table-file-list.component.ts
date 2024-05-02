import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
// Angular Material
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
// Library
import { DataService, GWCommon } from '@growthware/common/services';
import { LogLevel, LoggingService } from '@growthware/core/logging';
// Feature
import { IFileInfoLight } from '../../interfaces/file-info-light.model';
import { FileManagerService } from '../../file-manager.service';

@Component({
	selector: 'gw-core-table-file-list',
	standalone: true,
	imports: [
		MatFormFieldModule, 
		MatInputModule, 
		MatTableModule, 
		MatSortModule, 
		MatPaginatorModule
	],
	templateUrl: './table-file-list.component.html',
	styleUrl: './table-file-list.component.scss'
})
export class TableFileListComponent implements AfterViewInit, OnDestroy, OnInit {
	private _DataSubject = new BehaviorSubject<Array<IFileInfoLight>>([]);
	private _Subscriptions: Subscription = new Subscription();
  
	readonly data$ = this._DataSubject.asObservable();
	dataSource = new MatTableDataSource<IFileInfoLight>([]);
	// displayedColumns pulled from IFileInfoLight Note: order here effects the order in the table
	displayedColumns: string[] = [
		'fullName',
		'createdShort',
		'modifiedShort',
		'size'
	];
	id: string = '';

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private _DataSvc: DataService,
    private _FileManagerSvc: FileManagerService,
    private _GWCommon: GWCommon,
    private _LoggingSvc: LoggingService,
  ) { }

  ngOnDestroy(): void {
  	this._Subscriptions.unsubscribe();
  }

  ngOnInit(): void {
  	if(this._GWCommon.isNullOrUndefined(this.id)) {
  		this._LoggingSvc.toast('The id can not be blank!', 'File List Component', LogLevel.Error);
  	} else {
  		this._Subscriptions.add(this._FileManagerSvc.filesChanged$.subscribe((data: Array<IFileInfoLight>) => {
  			// console.log('TableFileListComponent.ngOnInit', data);
  			this.dataSource.data = data;
  			this.dataSource.paginator = this.paginator;
  			this.dataSource.sort = this.sort;
  		}));
  	}
  }

  ngAfterViewInit() {
  	this.dataSource.paginator = this.paginator;
  }

  applyFilter(event: Event) {
  	const filterValue = (event.target as HTMLInputElement).value;
  	this.dataSource.filter = filterValue.trim().toLowerCase();

  	if (this.dataSource.paginator) {
  		this.dataSource.paginator.firstPage();
  	}
  }
}
