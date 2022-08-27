import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminWorksampleCategoryCreateeditComponent } from './admin-worksample-category-createedit.component';

describe('AdminWorksampleCategoryCreateeditComponent', () => {
  let component: AdminWorksampleCategoryCreateeditComponent;
  let fixture: ComponentFixture<AdminWorksampleCategoryCreateeditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminWorksampleCategoryCreateeditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminWorksampleCategoryCreateeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
