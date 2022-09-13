import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-navbar',
  templateUrl: './user-navbar.component.html',
  styleUrls: ['./user-navbar.component.css']
})
export class UserNavbarComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  littleNav:boolean = true
  First:boolean = false
  First1:boolean = false
  Second:boolean = false
  rotateArrow1:string = 'rotate-arrow-back'
  rotateArrow11:string = 'rotate-arrow-back'
  rotateArrow2:string = 'rotate-arrow-back'
  rotateArrow3:string = 'rotate-arrow-back'
  rotateArrow4:string = 'rotate-arrow-back'
  openSubmenu1:string = 'open-submenu-back'
  openSubmenu11:string = 'open-submenu-back'
  openSubmenu2:string = 'open-submenu-back'
  openSubmenu3:string = 'open-submenu-back'
  openSubmenu4:string = 'open-submenu-back'

  LittleNav(){
    setTimeout(() => {
      this.littleNav = !this.littleNav
    }, 52);
  }

  first(){
    this.First = !this.First
    if(this.rotateArrow1 == 'rotate-arrow-back'){
      this.rotateArrow1 = 'rotate-arrow'
    }else{
      this.rotateArrow1 = 'rotate-arrow-back'
    }

    if(this.openSubmenu1 == 'open-submenu'){
      this.openSubmenu1 = 'open-submenu-back'
    }else{
      this.openSubmenu1 = 'open-submenu'
    }
  }
  first1(){
    this.First = !this.First
    if(this.rotateArrow11 == 'rotate-arrow-back'){
      this.rotateArrow11 = 'rotate-arrow'
    }else{
      this.rotateArrow11 = 'rotate-arrow-back'
    }

    if(this.openSubmenu11 == 'open-submenu'){
      this.openSubmenu11 = 'open-submenu-back'
    }else{
      this.openSubmenu11 = 'open-submenu'
    }
  }
  second(){
    this.First = !this.First
    if(this.rotateArrow2 == 'rotate-arrow-back'){
      this.rotateArrow2 = 'rotate-arrow'
    }else{
      this.rotateArrow2 = 'rotate-arrow-back'
    }

    if(this.openSubmenu2 == 'open-submenu'){
      this.openSubmenu2 = 'open-submenu-back'
    }else{
      this.openSubmenu2 = 'open-submenu'
    }
  }
  third(){
    this.First = !this.First
    if(this.rotateArrow3 == 'rotate-arrow-back'){
      this.rotateArrow3 = 'rotate-arrow'
    }else{
      this.rotateArrow3 = 'rotate-arrow-back'
    }

    if(this.openSubmenu3 == 'open-submenu'){
      this.openSubmenu3 = 'open-submenu-back'
    }else{
      this.openSubmenu3 = 'open-submenu'
    }
  }
  forth(){
    this.First = !this.First
    if(this.rotateArrow4 == 'rotate-arrow-back'){
      this.rotateArrow4 = 'rotate-arrow'
    }else{
      this.rotateArrow4 = 'rotate-arrow-back'
    }

    if(this.openSubmenu4 == 'open-submenu'){
      this.openSubmenu4 = 'open-submenu-back'
    }else{
      this.openSubmenu4 = 'open-submenu'
    }
  }

}
