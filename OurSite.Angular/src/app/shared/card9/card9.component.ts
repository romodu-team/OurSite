import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card9',
  templateUrl: './card9.component.html',
  styleUrls: ['./card9.component.css']
})
export class Card9Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  @Input() number:number = 1
  @Input() title:string = 'فایزه بلقان آبادی'
  @Input() description:string = 'ادمین' 
  @Input() image:string = ' ../../../assets/imgs/bill-gates-wealthiest-person.jpg '

}
