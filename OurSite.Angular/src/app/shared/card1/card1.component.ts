import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card1',
  templateUrl: './card1.component.html',
  styleUrls: ['./card1.component.css']
})
export class Card1Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  @Input() title:string = "طراحی اختصاصی و گرافیکی"
  @Input()  discription:string = 'شما  بااین یک متن آزمایشی برای مقالات  سیوی شما است. ما می‌توانیم با برسی سایت و محتوای شما باعث رشد وسایت شما در نتایج گوگل شویم'
  @Input() link:string = ""
  
  CardHover:string = 'bg-white'
  textHover:string = 'text-black'
  lineHover:string = 'background-color:rgb(254, 159, 18);'


  cardHover(){
    this.CardHover = 'bg-primary'
    this.textHover = 'text-white'
    this.lineHover = 'background-color:white;'
  }

  cardInHover(){
    this.CardHover = 'bg-white'
    this.textHover = 'text-black'
    this.lineHover = 'background-color:rgb(254, 159, 18);'

  }
}
