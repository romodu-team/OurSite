import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AdminUserProjectModalComponent } from './admin-user-project-modal/admin-user-project-modal.component';

@Component({
  selector: 'app-admin-user-project',
  templateUrl: './admin-user-project.component.html',
  styleUrls: ['./admin-user-project.component.css']
})
export class AdminUserProjectComponent implements OnInit {

  constructor(private dialog: MatDialog) { }

  ngOnInit(): void {
  }

  openDialog() {
    this.dialog.open(AdminUserProjectModalComponent);

  }

}
