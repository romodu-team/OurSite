import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTicketPriorityModalComponent } from './admin-ticket-priority-modal.component';

describe('AdminTicketPriorityModalComponent', () => {
  let component: AdminTicketPriorityModalComponent;
  let fixture: ComponentFixture<AdminTicketPriorityModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminTicketPriorityModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTicketPriorityModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
