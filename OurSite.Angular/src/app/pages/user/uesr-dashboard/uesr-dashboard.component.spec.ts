import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UesrDashboardComponent } from './uesr-dashboard.component';

describe('UesrDashboardComponent', () => {
  let component: UesrDashboardComponent;
  let fixture: ComponentFixture<UesrDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UesrDashboardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UesrDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
