import { Component, OnInit } from '@angular/core';
import { EssService } from 'src/app/shared/services/ess.service';

@Component({
  selector: 'app-admin-project-features',
  templateUrl: './admin-project-features.component.html',
  styleUrls: ['./admin-project-features.component.css']
})
export class AdminProjectFeaturesComponent implements OnInit {

  constructor(public essSer:EssService) { }

  ngOnInit(): void {
  }

}
