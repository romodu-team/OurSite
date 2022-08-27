import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminWorksampleCreateComponent } from './admin-worksample-create.component';

describe('AdminWorksampleCreateComponent', () => {
  let component: AdminWorksampleCreateComponent;
  let fixture: ComponentFixture<AdminWorksampleCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminWorksampleCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminWorksampleCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
