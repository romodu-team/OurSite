import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminProjectModalComponent } from './admin-project-modal.component';

describe('AdminProjectModalComponent', () => {
  let component: AdminProjectModalComponent;
  let fixture: ComponentFixture<AdminProjectModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminProjectModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminProjectModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
