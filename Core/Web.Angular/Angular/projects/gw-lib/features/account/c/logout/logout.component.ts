import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
// Feature
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-logout',
  standalone: true,
  imports: [
    CommonModule,
  ],
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {

  constructor(private _AccountSvc: AccountService) { }

  ngOnInit(): void {
    this._AccountSvc.logout();
  }

}
