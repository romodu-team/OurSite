import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCounselingCounselComponent } from './admin-counseling-counsel.component';

describe('AdminCounselingCounselComponent', () => {
  let component: AdminCounselingCounselComponent;
  let fixture: ComponentFixture<AdminCounselingCounselComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCounselingCounselComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCounselingCounselComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
