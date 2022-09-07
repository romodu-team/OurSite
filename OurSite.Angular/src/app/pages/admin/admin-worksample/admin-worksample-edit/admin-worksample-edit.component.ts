import { Component, OnInit } from '@angular/core';
import { EssService } from 'src/app/shared/services/ess.service';

@Component({
  selector: 'app-admin-worksample-edit',
  templateUrl: './admin-worksample-edit.component.html',
  styleUrls: ['./admin-worksample-edit.component.css']
})
export class AdminWorksampleEditComponent implements OnInit {

  constructor(public essSer:EssService) { }

  ngOnInit(): void {
  }

}
