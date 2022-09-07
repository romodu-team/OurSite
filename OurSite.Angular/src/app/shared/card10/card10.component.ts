import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-card10',
  templateUrl: './card10.component.html',
  styleUrls: ['./card10.component.css']
})
export class Card10Component implements OnInit {

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  @Input() title:string = 'اسنپ'
  @Input() description:string = 'شرکت ایده گزین ارتباطات روماک'
  @Input() condition:string = 'در حال انجام'
  @Input() link:string = 'www.sanp.ir'
  @Input() openDialog!: () => void
  @Input() editUrl:string = ''
}
