import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminManagementModalComponent } from './admin-management-modal.component';

describe('AdminManagementModalComponent', () => {
  let component: AdminManagementModalComponent;
  let fixture: ComponentFixture<AdminManagementModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminManagementModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminManagementModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
