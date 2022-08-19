import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient,
              private router:Router) { }

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
        this.userLoggedIn = true
        this.router.navigate(['/user'])
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
}
