import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRoleManagementAccessComponent } from './admin-role-management-access.component';

describe('AdminRoleManagementAccessComponent', () => {
  let component: AdminRoleManagementAccessComponent;
  let fixture: ComponentFixture<AdminRoleManagementAccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminRoleManagementAccessComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminRoleManagementAccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
