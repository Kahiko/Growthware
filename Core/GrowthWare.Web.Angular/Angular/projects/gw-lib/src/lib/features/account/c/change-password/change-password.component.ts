import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
// Feature
import { AccountService } from '../../account.service';

@Component({
  selector: 'gw-lib-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  private _Return: string = '';

  constructor(
    private _AccountSvc: AccountService,
    private _Router: Router,
    private _ActivatedRoute: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this._ActivatedRoute.queryParams.subscribe((params) => {
      this._Return = params['return'] || '/home';
    });
  }

  changePassword() {
    // logic to change password
    // on success change routes
    this._Router.navigateByUrl(this._Return);
  }

}
