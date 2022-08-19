import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  constructor(private apiSer:ApiService,
              private router:Router) { }

  userInformation!: Observable<any>

  ngOnInit(): void {
    this.userInformation = this.apiSer.getUserInformation()
  }

  logout(){
    localStorage.clear()
    this.router.navigate(['/'])
  }

}
