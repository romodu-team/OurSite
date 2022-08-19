import { Component, Input, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Observable , of } from 'rxjs';
import { Card2Component } from 'src/app/shared/card2/card2.component';

@Component({
  selector: 'app-feature',
  templateUrl: './feature.component.html',
  styleUrls: ['./feature.component.css']
})
export class FeatureComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  flag:boolean = true

  chengCardStyle(){
    this.flag = !this.flag
  }

  selectedOptions = new Set()

  
  Cards$:Observable<any[]> = of(
    [
      {
        title:'طراحی اقتصادی',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'طراحی گرافیکی',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'هدر',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'فوتر',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'تیکت',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'دسترسی بالا',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'فروشگاه',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'بازی آنلاین',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'پشتیبانی',
        discription:'k,jgcqwuegdique'
      },
    ]
  )

  addFeature(title:string, discription:string){

    if(!this.selectedOptions.has(title)){
      this.selectedOptions.add(title)
    }else{
      this.selectedOptions.delete(title)
    }
    console.log(this.selectedOptions);
  }

  reset(){
    this.selectedOptions.clear()
    window.location.reload()
  }

}
