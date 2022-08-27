import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTicketStatusModalComponent } from './admin-ticket-status-modal.component';

describe('AdminTicketStatusModalComponent', () => {
  let component: AdminTicketStatusModalComponent;
  let fixture: ComponentFixture<AdminTicketStatusModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminTicketStatusModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTicketStatusModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
