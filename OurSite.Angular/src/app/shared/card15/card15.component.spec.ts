import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Card15Component } from './card15.component';

describe('Card15Component', () => {
  let component: Card15Component;
  let fixture: ComponentFixture<Card15Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Card15Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Card15Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
