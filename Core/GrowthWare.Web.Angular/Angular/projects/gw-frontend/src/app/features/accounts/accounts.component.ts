import { Component, OnInit } from '@angular/core';
import { GWLibSearchService, SearchCriteria } from 'projects/gw-lib/src/public-api';
import { GWLibDynamicTableService } from 'projects/gw-lib/src/public-api';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss']
})
export class AccountsComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {

  }

}
