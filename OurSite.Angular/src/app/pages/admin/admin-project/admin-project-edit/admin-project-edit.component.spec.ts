import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminProjectEditComponent } from './admin-project-edit.component';

describe('AdminProjectEditComponent', () => {
  let component: AdminProjectEditComponent;
  let fixture: ComponentFixture<AdminProjectEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminProjectEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminProjectEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
