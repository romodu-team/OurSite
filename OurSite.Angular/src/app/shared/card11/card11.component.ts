import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card11',
  templateUrl: './card11.component.html',
  styleUrls: ['./card11.component.css']
})
export class Card11Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  @Input() number:number = 1
  @Input() title:string ='تعریف پروژه'
  @Input() description:string ='تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی، و فرهنگ پیشرو در زبان فارسی ایجاد کرد'

}
