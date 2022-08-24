import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUserProjectEditUserComponent } from './admin-user-project-edit-user.component';

describe('AdminUserProjectEditUserComponent', () => {
  let component: AdminUserProjectEditUserComponent;
  let fixture: ComponentFixture<AdminUserProjectEditUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminUserProjectEditUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUserProjectEditUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
