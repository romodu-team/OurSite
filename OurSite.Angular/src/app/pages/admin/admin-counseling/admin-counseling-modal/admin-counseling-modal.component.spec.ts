import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCounselingModalComponent } from './admin-counseling-modal.component';

describe('AdminCounselingModalComponent', () => {
  let component: AdminCounselingModalComponent;
  let fixture: ComponentFixture<AdminCounselingModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCounselingModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCounselingModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
