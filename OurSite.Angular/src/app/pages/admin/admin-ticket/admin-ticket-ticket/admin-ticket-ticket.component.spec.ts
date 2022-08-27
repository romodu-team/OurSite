import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTicketTicketComponent } from './admin-ticket-ticket.component';

describe('AdminTicketTicketComponent', () => {
  let component: AdminTicketTicketComponent;
  let fixture: ComponentFixture<AdminTicketTicketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminTicketTicketComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTicketTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
