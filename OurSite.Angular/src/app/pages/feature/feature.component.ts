import { Component, Input, OnInit } from '@angular/core';
import { Observable , of } from 'rxjs';

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
  
  Cards$:Observable<any[]> = of(
    [
      {
        title:'lkjl.f',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'lkjl.f',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'lkjl.f',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'lkjl.f',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'lkjl.f',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'lkjl.f',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'lkjl.f',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'lkjl.f',
        discription:'k,jgcqwuegdique'
      },
      {
        title:'lkjl.f',
        discription:'k,jgcqwuegdique'
      },
    ]
  )

}
