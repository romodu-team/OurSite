import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TestService {

  constructor(private http:HttpClient) { }

  test(){
    return this.http.get('http://localhost:7181/api/Admin/view-Roles').subscribe(data => {
      console.log(data);
    })
  }
}
