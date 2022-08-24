import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRoleManagementModalComponent } from './admin-role-management-modal.component';

describe('AdminRoleManagementModalComponent', () => {
  let component: AdminRoleManagementModalComponent;
  let fixture: ComponentFixture<AdminRoleManagementModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminRoleManagementModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminRoleManagementModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
