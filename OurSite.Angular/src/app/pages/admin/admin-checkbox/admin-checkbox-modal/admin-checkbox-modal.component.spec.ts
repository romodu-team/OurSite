import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCheckboxModalComponent } from './admin-checkbox-modal.component';

describe('AdminCheckboxModalComponent', () => {
  let component: AdminCheckboxModalComponent;
  let fixture: ComponentFixture<AdminCheckboxModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCheckboxModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCheckboxModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
