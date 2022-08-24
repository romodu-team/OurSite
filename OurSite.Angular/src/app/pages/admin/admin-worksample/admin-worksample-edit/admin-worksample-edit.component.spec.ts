import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminWorksampleEditComponent } from './admin-worksample-edit.component';

describe('AdminWorksampleEditComponent', () => {
  let component: AdminWorksampleEditComponent;
  let fixture: ComponentFixture<AdminWorksampleEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminWorksampleEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminWorksampleEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
