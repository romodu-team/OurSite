import { Component, Input, OnInit } from '@angular/core';
import { Observable , of } from 'rxjs';

@Component({
  selector: 'app-demo',
  templateUrl: './demo.component.html',
  styleUrls: ['./demo.component.css']
})
export class DemoComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
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
