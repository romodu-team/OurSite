import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UesrProjectComponent } from './uesr-project.component';

describe('UesrProjectComponent', () => {
  let component: UesrProjectComponent;
  let fixture: ComponentFixture<UesrProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UesrProjectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UesrProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
