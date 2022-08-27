import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserTicketModalComponent } from './user-ticket-modal.component';

describe('UserTicketModalComponent', () => {
  let component: UserTicketModalComponent;
  let fixture: ComponentFixture<UserTicketModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserTicketModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserTicketModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
