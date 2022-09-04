import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card5',
  templateUrl: './card5.component.html',
  styleUrls: ['./card5.component.css']
})
export class Card5Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  @Input() title:string = 'صفحه اصلی'

  hover:string = 'bg-white'
  shadow:string = 'shadow-sm'
  color:string = 'm'

  Hover(){
    this.hover = 'bg-primary'
    this.shadow = 'shadow-lg'
  }

  InHover(){
    this.hover = 'bg-white'
    this.shadow = 'shadow-sm'
  }

  Color(){
    this.color = 'text-white'
  }

  InColor(){
    this.color = 'm'
  }

}
