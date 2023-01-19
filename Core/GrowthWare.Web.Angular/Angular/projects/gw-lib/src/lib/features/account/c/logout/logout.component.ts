import { Component, OnInit } from '@angular/core';
// Feature
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  constructor(private _AccountSvc: AccountService) { }

  ngOnInit(): void {
    this._AccountSvc.logout();
  }

}
