import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card8',
  templateUrl: './card8.component.html',
  styleUrls: ['./card8.component.css']
})
export class Card8Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  @Input() head:string = 'فایزه بلقان آبادی'
  @Input() title1:string = 'درخواست نامه برای طراحی سایت'
  @Input() title2:string = 'طراحی سایت'
  @Input() description:string = 'لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز، و کاربردهای متنوع با هدف بهبود ابزارهای کارب فرای، و ف کرد'
  @Input() image:string = '../../../assets/imgs/bill-gates-wealthiest-person.jpg'

}
