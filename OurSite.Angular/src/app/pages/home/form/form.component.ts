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
    let formdata = new FormData()
    formdata.append('firstName', this.myForm.value.firstName)
    formdata.append('lastName', this.myForm.value.lastName)
    formdata.append('email', this.myForm.value.email)
    formdata.append('phoneNumber', this.myForm.value.phone)
    formdata.append('content', this.myForm.value.description)
    this.apiSer.postHomeForm(formdata)


  }

}
