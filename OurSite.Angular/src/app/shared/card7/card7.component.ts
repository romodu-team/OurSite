import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card7',
  templateUrl: './card7.component.html',
  styleUrls: ['./card7.component.css']
})
export class Card7Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  @Input() img:string = '../../../assets/imgs/chart-histogram.png'
  @Input() title:string = 'digikala'

  textHover:string = 'bg-white text-black'

  caredHover(){
    this.textHover = 'bg-primary text-white'
  }

  caredInHover(){
    this.textHover = 'bg-white text-black'
  }

}
