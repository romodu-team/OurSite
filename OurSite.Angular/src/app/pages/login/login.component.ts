import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private authSer:AuthService) { }

  ngOnInit(): void {
  }

  registerForm = new FormGroup({
    name: new FormControl(''),
    family: new FormControl(''),
    userName: new FormControl(''),
    password: new FormControl(''),
    conmfrimPassword: new FormControl(''),
    mobile: new FormControl(''),
    email: new FormControl(''),
    accountType: new FormControl('null')
  })

  loginForm = new FormGroup({
    userName: new FormControl(''),
    password: new FormControl('')
  })

  regesterUser(){
    this.authSer.registerUser({
      name: this.registerForm.value.name,
      family: this.registerForm.value.family,
      userName: this.registerForm.value.userName,
      password: this.registerForm.value.password,
      conmfrimPassword: this.registerForm.value.conmfrimPassword,
      mobile: this.registerForm.value.mobile,
      email: this.registerForm.value.email,
      accountType: +this.registerForm.value.accountType
    })
  }

  login(){
    this.authSer.login({
      userName: this.loginForm.value.userName,
      password: this.loginForm.value.password
    })
  }

}
