import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUserProjectComponent } from './admin-user-project.component';

describe('AdminUserProjectComponent', () => {
  let component: AdminUserProjectComponent;
  let fixture: ComponentFixture<AdminUserProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminUserProjectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUserProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
