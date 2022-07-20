import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-slider1',
  templateUrl: './slider1.component.html',
  styleUrls: ['./slider1.component.css']
})
export class Slider1Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  selectedImg: any = {
    id:1,
    url: '../../../assets/imgs/header.png'
  }

  photos:any[] = [
    {
      id:1,
      url: '../../../assets/imgs/header.png'
    },
    {
      id:2,
      url: '../../../assets/imgs/Shot 0088.png'
    },
    {
      id:3,
      url: '../../../assets/imgs/header.png'
    },
    {
      id:4,
      url: '../../../assets/imgs/Shot 0088.png'
    },
  ]

  selectedPhoto(id:number){
    for (let i = 0; i < this.photos.length; i++) {
      if(this.photos[i].id == id ){
        this.selectedImg = this.photos[i]
        console.log(id)
      }
    }
  }


}
