import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'gw-lib-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  get getControls() {
    return this.loginForm.controls;
  }

  constructor(private _FormBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.loginForm = this._FormBuilder.group({
      account: ['', [Validators.required, Validators.minLength(4)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required]],
      message: ['', [Validators.required]]
    })
  }

  getErrorMessage() {
    if (this.loginForm.controls['email'].hasError('required')) {
      return 'You must enter a value';
    }
    return this.loginForm.controls['email'] ? 'Not a valid email' : '';
  }

  onSubmit(){
    console.log(this.loginForm);
  }

}
