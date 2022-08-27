import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCountactCountactComponent } from './admin-countact-countact.component';

describe('AdminCountactCountactComponent', () => {
  let component: AdminCountactCountactComponent;
  let fixture: ComponentFixture<AdminCountactCountactComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCountactCountactComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCountactCountactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
