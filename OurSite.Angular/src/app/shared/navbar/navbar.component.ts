import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(public authSer:AuthService) { }
  
  name:string = ''
  ngOnInit(): void {
    this.name = localStorage.getItem('name')!
  }

  @Input() dark = true
  

}
