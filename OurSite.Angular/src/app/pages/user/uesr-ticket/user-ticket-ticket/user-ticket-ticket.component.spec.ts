import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserTicketTicketComponent } from './user-ticket-ticket.component';

describe('UserTicketTicketComponent', () => {
  let component: UserTicketTicketComponent;
  let fixture: ComponentFixture<UserTicketTicketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserTicketTicketComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserTicketTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
