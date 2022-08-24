import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FillterComponent } from './fillter.component';

describe('FillterComponent', () => {
  let component: FillterComponent;
  let fixture: ComponentFixture<FillterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FillterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FillterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
