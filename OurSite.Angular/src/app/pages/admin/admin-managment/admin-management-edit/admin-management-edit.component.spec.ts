import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminManagementEditComponent } from './admin-management-edit.component';

describe('AdminManagementEditComponent', () => {
  let component: AdminManagementEditComponent;
  let fixture: ComponentFixture<AdminManagementEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminManagementEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminManagementEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
