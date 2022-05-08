import { Component, OnInit } from '@angular/core';
import { GWLibSearchService } from 'projects/gw-lib/src/public-api';
import { GWLibDynamicTableService } from 'projects/gw-lib/src/public-api';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss']
})
export class AccountsComponent implements OnInit {


  constructor(private _SearchSvc: GWLibSearchService, private _DynamicTableSvc: GWLibDynamicTableService) { }

  ngOnInit(): void {
  }

}
