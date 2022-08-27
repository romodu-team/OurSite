import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCountactNewComponent } from './admin-countact-new.component';

describe('AdminCountactNewComponent', () => {
  let component: AdminCountactNewComponent;
  let fixture: ComponentFixture<AdminCountactNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCountactNewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCountactNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
