import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCountactComponent } from './admin-countact.component';

describe('AdminCountactComponent', () => {
  let component: AdminCountactComponent;
  let fixture: ComponentFixture<AdminCountactComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCountactComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCountactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
