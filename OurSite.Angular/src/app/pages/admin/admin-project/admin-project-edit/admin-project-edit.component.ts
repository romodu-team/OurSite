import { Component, OnInit } from '@angular/core';
import { EssService } from 'src/app/shared/services/ess.service';

@Component({
  selector: 'app-admin-project-edit',
  templateUrl: './admin-project-edit.component.html',
  styleUrls: ['./admin-project-edit.component.css']
})
export class AdminProjectEditComponent implements OnInit {

  constructor(public essSer:EssService) { }

  ngOnInit(): void {
  }



}
