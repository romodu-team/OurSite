import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTicketCategoryComponent } from './admin-ticket-category.component';

describe('AdminTicketCategoryComponent', () => {
  let component: AdminTicketCategoryComponent;
  let fixture: ComponentFixture<AdminTicketCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminTicketCategoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTicketCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
