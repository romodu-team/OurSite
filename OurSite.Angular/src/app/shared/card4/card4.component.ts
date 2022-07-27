import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card4',
  templateUrl: './card4.component.html',
  styleUrls: ['./card4.component.css']
})
export class Card4Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  @Input() url:string = '../../../assets/imgs/Logo-Nicode copy-fin3.png'
  @Input() title:string = 'تیم سیپا'
  @Input() siteLink:string = 'www.nicode.com'
  @Input() like:string = '999'

}
