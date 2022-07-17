import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card2',
  templateUrl: './card2.component.html',
  styleUrls: ['./card2.component.css']
})
export class Card2Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  @Input() title:string ='هدر شفاف'
  @Input() discription:string = 'هدر شفاف به همراه اسلایدر با تون رنگی متناسب با صفحات سایت هدر شفاف به همراه اسلایدر با تون رنگی متناسب با صفحات سایت'

  textHover :string = 'text-black'
  bgHover : string = 'bg-white'

  CardHover(){
    this.textHover = 'text-white'
    this.bgHover = 'bg-success'
  }

  CardInHover(){
    this.textHover = 'text-black'
    this.bgHover = 'bg-white'
  }
}
