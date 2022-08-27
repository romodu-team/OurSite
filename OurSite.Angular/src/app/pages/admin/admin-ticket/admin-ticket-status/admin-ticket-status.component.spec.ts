import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTicketStatusComponent } from './admin-ticket-status.component';

describe('AdminTicketStatusComponent', () => {
  let component: AdminTicketStatusComponent;
  let fixture: ComponentFixture<AdminTicketStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminTicketStatusComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTicketStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
