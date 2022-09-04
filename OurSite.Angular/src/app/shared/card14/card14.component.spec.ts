import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Card14Component } from './card14.component';

describe('Card14Component', () => {
  let component: Card14Component;
  let fixture: ComponentFixture<Card14Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Card14Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Card14Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
