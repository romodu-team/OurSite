import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent implements OnInit {

  constructor(private apiSer:ApiService) { }

  myForm = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    email: new FormControl(''),
    phone: new FormControl(''),
    description: new FormControl(''),
  })

  ngOnInit(): void {
  }

  submitForm(){
    // let form = {
    //   UserFullName: this.myForm.value.firstName + this.myForm.value.lastName,
    //   UserEmail: this.myForm.value.email,
    //   UserPhoneNumber: this.myForm.value.phone,
    //   Expration: this.myForm.value.description
    // }
    let formdata = new FormData()
    formdata.append('UserFullName', this.myForm.value.firstName + this.myForm.value.lastName)
    formdata.append('UserEmail', this.myForm.value.email)
    formdata.append('UserPhoneNumber', this.myForm.value.phone)
    formdata.append('Expration', this.myForm.value.description)
    this.apiSer.postHomeForm(formdata)
  }

}
