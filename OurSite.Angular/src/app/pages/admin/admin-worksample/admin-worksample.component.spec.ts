import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminWorksampleComponent } from './admin-worksample.component';

describe('AdminWorksampleComponent', () => {
  let component: AdminWorksampleComponent;
  let fixture: ComponentFixture<AdminWorksampleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminWorksampleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminWorksampleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
