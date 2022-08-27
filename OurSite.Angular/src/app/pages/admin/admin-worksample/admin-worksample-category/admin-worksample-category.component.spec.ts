import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminWorksampleCategoryComponent } from './admin-worksample-category.component';

describe('AdminWorksampleCategoryComponent', () => {
  let component: AdminWorksampleCategoryComponent;
  let fixture: ComponentFixture<AdminWorksampleCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminWorksampleCategoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminWorksampleCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
