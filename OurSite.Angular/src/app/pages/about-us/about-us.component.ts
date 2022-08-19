import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-about-us',
  templateUrl: './about-us.component.html',
  styleUrls: ['./about-us.component.css']
})
export class AboutUsComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  filter:string = 'filter: brightness(100%);'

  hover(){
    this.filter = 'filter: brightness(65%);'
  }
  inHover(){
    this.filter = 'filter: brightness(100%);'
  }

}
