import { Component, OnInit } from '@angular/core';
import { AuthService } from './shared/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  
  constructor(private authSer:AuthService) {}
  ngOnInit(): void {
      this.checkAuth()
  }

  checkAuth(){
    if(localStorage.getItem('userToken')){
      this.authSer.userLoggedIn = true
    }else{
      this.authSer.userLoggedIn = false
    }
  }

}