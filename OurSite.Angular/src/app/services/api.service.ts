import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';


@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  postHomeForm(body: any){
    return this.http.post(environment.domain + 'api/ConsultationRequest/send-form-with-file', body , {headers: {'Content-Type': 'multipart/form-data'}}).subscribe(data => {
      console.log(data);
    })
  }
}