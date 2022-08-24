import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UesrProfileComponent } from './uesr-profile.component';

describe('UesrProfileComponent', () => {
  let component: UesrProfileComponent;
  let fixture: ComponentFixture<UesrProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UesrProfileComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UesrProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
