import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminProjectFeaturesComponent } from './admin-project-features.component';

describe('AdminProjectFeaturesComponent', () => {
  let component: AdminProjectFeaturesComponent;
  let fixture: ComponentFixture<AdminProjectFeaturesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminProjectFeaturesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminProjectFeaturesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
