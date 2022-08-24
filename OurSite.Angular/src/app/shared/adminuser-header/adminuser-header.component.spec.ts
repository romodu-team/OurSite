import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminuserHeaderComponent } from './adminuser-header.component';

describe('AdminuserHeaderComponent', () => {
  let component: AdminuserHeaderComponent;
  let fixture: ComponentFixture<AdminuserHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminuserHeaderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminuserHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
