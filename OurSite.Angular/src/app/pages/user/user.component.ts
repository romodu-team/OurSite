import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ApiService } from 'src/app/shared/services/api.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  constructor(private apiSer:ApiService,
              private authSer:AuthService) { }

  userInformation!: Observable<any>

  ngOnInit(): void {
    this.userInformation = this.apiSer.getUserInformation()
  }

  logout(){
    this.authSer.logout()
  }

}
