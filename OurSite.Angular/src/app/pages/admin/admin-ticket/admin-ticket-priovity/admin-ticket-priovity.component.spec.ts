import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTicketPriovityComponent } from './admin-ticket-priovity.component';

describe('AdminTicketPriovityComponent', () => {
  let component: AdminTicketPriovityComponent;
  let fixture: ComponentFixture<AdminTicketPriovityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminTicketPriovityComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTicketPriovityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
