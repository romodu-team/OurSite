import { Component, Input, OnInit } from '@angular/core';
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

  @Input() pageTitle:string = 'داشبورد'
  @Input() btnTitle:string = 'ثبت'
  @Input() headerFunction1!: () => void
  @Input() headerFunction2!: () => void

}
