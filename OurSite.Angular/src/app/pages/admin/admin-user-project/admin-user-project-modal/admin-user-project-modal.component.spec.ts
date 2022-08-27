import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUserProjectModalComponent } from './admin-user-project-modal.component';

describe('AdminUserProjectModalComponent', () => {
  let component: AdminUserProjectModalComponent;
  let fixture: ComponentFixture<AdminUserProjectModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminUserProjectModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUserProjectModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
