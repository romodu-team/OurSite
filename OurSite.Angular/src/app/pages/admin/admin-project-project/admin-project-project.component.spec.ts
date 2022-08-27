import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminProjectProjectComponent } from './admin-project-project.component';

describe('AdminProjectProjectComponent', () => {
  let component: AdminProjectProjectComponent;
  let fixture: ComponentFixture<AdminProjectProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminProjectProjectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminProjectProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
