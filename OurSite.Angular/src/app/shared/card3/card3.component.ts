import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card3',
  templateUrl: './card3.component.html',
  styleUrls: ['./card3.component.css']
})
export class Card3Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  @Input() title:string = 'هدر شفاف'
  @Input() discription:string = 'هدر شفاف به همراه اسلایدر با تون رنگی متناسب با صفحات سایت شما'

  bgHover :string = 'bg-white'
  textHover : string = 'text-black'

  cardHover(){
    this.bgHover = 'bg-success'
    this.textHover = 'text-white'
  }
  cardInHover(){
    this.bgHover = 'bg-white'
    this.textHover = 'text-black'
  }

}
