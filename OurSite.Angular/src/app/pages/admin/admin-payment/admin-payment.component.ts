import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AdminPaymentModalComponent } from './admin-payment-modal/admin-payment-modal.component';

@Component({
  selector: 'app-admin-payment',
  templateUrl: './admin-payment.component.html',
  styleUrls: ['./admin-payment.component.css']
})
export class AdminPaymentComponent implements OnInit {

  constructor(private dialog:MatDialog) { }

  ngOnInit(): void {
  }

  openDialog() {
    this.dialog.open(AdminPaymentModalComponent, {width:'40%'});
  }

}
