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

  @Input() img:string = '../../../assets/imgs/chart-histogram.png'
  @Input() title:string = 'طراحی اختصاصی'



}
