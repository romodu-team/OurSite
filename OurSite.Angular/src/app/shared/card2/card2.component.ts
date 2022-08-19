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
  @Input() more:string = 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.'

  select:string = 'bg-white text-black'
  showDiscription:boolean = false
  arrowAnimation:string = 'cancle-arrow-animation'
  arrowLabel:string = 'show-arrow-label'
  cardShadow:string = 'shadow-sm'

  shadow(){
    this.cardShadow = 'shadow-lg'
  }

  inShadow(){
    this.cardShadow = 'shadow-sm'
  }
  
  
  onSelect(){
    if(this.select == 'bg-white text-black'){
      this.select = 'bg-success text-white'
    }else{
      this.select = 'bg-white text-black'
    }
  }

  onDiscription(){
    this.showDiscription = !this.showDiscription
    if(this.arrowAnimation == 'cancle-arrow-animation'){
      this.arrowAnimation = 'arrow-animation'
      this.arrowLabel = 'hide-arrow-label'
    }else{
      this.arrowAnimation = 'cancle-arrow-animation'
      this.arrowLabel = 'show-arrow-label'
    }
  }
}
