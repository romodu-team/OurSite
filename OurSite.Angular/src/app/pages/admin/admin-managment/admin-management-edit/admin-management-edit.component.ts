import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-management-edit',
  templateUrl: './admin-management-edit.component.html',
  styleUrls: ['./admin-management-edit.component.css']
})
export class AdminManagementEditComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  flag:boolean = false
  number: number = 1
  isDisabled : boolean = true;

  edit(){
    this.flag = true
  }

  next(){
    this.number++
  }

  past(){
    this.number--
  }

  editAdmin(){

  }

}
