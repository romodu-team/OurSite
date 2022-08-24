import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUserProjectNewUserComponent } from './admin-user-project-new-user.component';

describe('AdminUserProjectNewUserComponent', () => {
  let component: AdminUserProjectNewUserComponent;
  let fixture: ComponentFixture<AdminUserProjectNewUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminUserProjectNewUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUserProjectNewUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
