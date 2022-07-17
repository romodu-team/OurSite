import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-demo-main',
  templateUrl: './demo-main.component.html',
  styleUrls: ['./demo-main.component.css']
})
export class DemoMainComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  cards$: Observable<any[]> = of([
    {
      title:'نیکد',
      like:'45',
      url:'www.nicode.com'
    },
    {
      title:'مبین',
      like:'999',
      url:'www.mobin.com'
    },
    {
      title:'نیکد',
      like:'45',
      url:'www.nicode.com'
    },
    {
      title:'نیکد',
      like:'45',
      url:'www.nicode.com'
    },
  ])

}
