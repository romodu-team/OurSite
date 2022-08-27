import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCheckboxComponent } from './admin-checkbox.component';

describe('AdminCheckboxComponent', () => {
  let component: AdminCheckboxComponent;
  let fixture: ComponentFixture<AdminCheckboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCheckboxComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCheckboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
