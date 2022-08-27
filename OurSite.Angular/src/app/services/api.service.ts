import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { map } from "rxjs";


@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  postHomeForm(body: any){
    return this.http.post<any>(environment.ROOT_PATH + 'api/ConsultationRequest/send-form-with-file', body ).subscribe(data => {
      alert(data.message)
      console.log(data);
    })
  }

  getUserInformation(){
    return this.http.get<any>(environment.ROOT_PATH + 'api/User/View-Profile/' + localStorage.getItem('userId')).pipe(
      map(user => user.data)
    )
  }

}