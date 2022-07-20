import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card6',
  templateUrl: './card6.component.html',
  styleUrls: ['./card6.component.css']
})
export class Card6Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  @Input() img:string = '../../../assets/imgs/UI@2x.ico'
  @Input() title:string = 'طراحی اختصاصی'

  topHover:string = 'margin-top:0px'
  cardHover:string = 'bg-white text-black'

  CardHover(){
    this.cardHover = 'bg-primary text-white'
    this.topHover = 'margin-top:-3px'

  }

  CardInHover(){
    this.cardHover = 'bg-white text-black'
    this.topHover = 'margin-top:0px'
  }



}
