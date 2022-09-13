import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public forecasts?: CheckBoxDto;

  constructor(http: HttpClient) {
    http.get<CheckBoxDto>('https://localhost:7181/api/CheckBox/get-all-CheckBox?sectionId=0').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }

  title = 'OurSite.FrontAngular';
}

interface CheckBoxDto {
  status: number;
  data: [{
    id: number,
    title: string,
    description: string,
    siteSectionName: string,
    iconName: string
  }];
  message: string;
}
