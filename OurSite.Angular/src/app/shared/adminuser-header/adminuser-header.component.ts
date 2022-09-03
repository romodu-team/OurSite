import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-adminuser-header',
  templateUrl: './adminuser-header.component.html',
  styleUrls: ['./adminuser-header.component.css']
})
export class AdminuserHeaderComponent implements OnInit {

  constructor(public authSer:AuthService) { }

  ngOnInit(): void {
  }

}
