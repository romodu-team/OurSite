import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment.prod';
import { Location } from "@angular/common";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient,
              private router:Router,
              private _loc:Location) { }

  userLoggedIn:boolean = false


  registerUser(body:any){
    return this.http.post<any>(environment.ROOT_PATH + 'api/User/signUp-user', body).subscribe(
      data => {
        alert(data.message)
      },
      err => {
        console.log(err);
      }
    )
  }

  login(body:any){
    return this.http.post<any>(environment.ROOT_PATH + 'api/User/login', body).subscribe(
      data => {
        alert(data.message)

        localStorage.setItem('userToken', data.data.token)
        localStorage.setItem('userId', data.data.userId)
        localStorage.setItem('name', data.data.firstName + ' ' + data.data.lastName)
        this.userLoggedIn = true
        this._loc.back()
      },
      err => {
        this.userLoggedIn = false
        console.log(err);
      }
    )
  }

  userAuthCheck(){
    return new Promise((resolve, reject) => {
      resolve(this.userLoggedIn)
    })
  }

  logout(){
    localStorage.clear()
    this.router.navigate(['/'])
    window.location.reload()
  }
}
