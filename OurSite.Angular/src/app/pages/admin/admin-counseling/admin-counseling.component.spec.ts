import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCounselingComponent } from './admin-counseling.component';

describe('AdminCounselingComponent', () => {
  let component: AdminCounselingComponent;
  let fixture: ComponentFixture<AdminCounselingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCounselingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCounselingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
