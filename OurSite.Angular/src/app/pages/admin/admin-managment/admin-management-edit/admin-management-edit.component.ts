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

  
  number: number = 1
  isDisabled : boolean = true;

  next(){
    this.number++
  }

  past(){
    this.number--
  }

  editAdmin(){

  }

}
