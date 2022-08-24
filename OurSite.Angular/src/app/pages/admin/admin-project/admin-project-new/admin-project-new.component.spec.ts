import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminProjectNewComponent } from './admin-project-new.component';

describe('AdminProjectNewComponent', () => {
  let component: AdminProjectNewComponent;
  let fixture: ComponentFixture<AdminProjectNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminProjectNewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminProjectNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
