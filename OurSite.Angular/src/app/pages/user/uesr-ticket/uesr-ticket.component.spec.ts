import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UesrTicketComponent } from './uesr-ticket.component';

describe('UesrTicketComponent', () => {
  let component: UesrTicketComponent;
  let fixture: ComponentFixture<UesrTicketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UesrTicketComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UesrTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
