import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-adminuser-navbar',
  templateUrl: './adminuser-navbar.component.html',
  styleUrls: ['./adminuser-navbar.component.css']
})
export class AdminuserNavbarComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  First:boolean = false
  rotateArrow:string = 'rotate-arrow-back'
  openSubmenu:string = 'open-submenu-back'

  first(){
    this.First = !this.First
    if(this.rotateArrow == 'rotate-arrow-back'){
      this.rotateArrow = 'rotate-arrow'
    }else{
      this.rotateArrow = 'rotate-arrow-back'
    }

    if(this.openSubmenu == 'open-submenu'){
      this.openSubmenu = 'open-submenu-back'
    }else{
      this.openSubmenu = 'open-submenu'
    }
  }

}
