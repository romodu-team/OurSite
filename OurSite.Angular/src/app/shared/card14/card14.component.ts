import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card14',
  templateUrl: './card14.component.html',
  styleUrls: ['./card14.component.css']
})
export class Card14Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  @Input() name:string = 'فایزه'
  @Input() userName:string = 'فایزه'
  @Input() loginDate:string = '5/12/1401'
  @Input() deposit:string = '100'
  @Input() number:string = '09154710879'
  @Input() grouping:string = 'طلایی'
  @Input() condition:string = 'پرداخت شده'
  @Input() projectName:string = 'اسم پروژ ای که دارد'
  @Input() description:string ='لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز، و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد، کتابهای زیادی در شصت و سه درصد گذشته حال و آینده، شناخت فراوان جامعه و متخصصان را می طلبد، تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی، و فرهنگ پیشرو در زبان فارسی ایجاد کرد، در این صورت می توان امید داشت که تمام و دشواری موجود در ارائه راهکارها، و شرایط سخت تایپ به پایان رسد و زمان مورد نیاز شامل حروفچینی دستاوردهای اصلی، و جوابگوی سوالات پیوسته اهل دنیای موجود طراحی اساسا مورد استفاده قرار گیرد'


}
