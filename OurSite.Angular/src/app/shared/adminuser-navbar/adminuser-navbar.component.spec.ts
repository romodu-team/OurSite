import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminuserNavbarComponent } from './adminuser-navbar.component';

describe('AdminuserNavbarComponent', () => {
  let component: AdminuserNavbarComponent;
  let fixture: ComponentFixture<AdminuserNavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminuserNavbarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminuserNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
