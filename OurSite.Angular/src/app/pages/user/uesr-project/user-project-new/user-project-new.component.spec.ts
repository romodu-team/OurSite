import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProjectNewComponent } from './user-project-new.component';

describe('UserProjectNewComponent', () => {
  let component: UserProjectNewComponent;
  let fixture: ComponentFixture<UserProjectNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserProjectNewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserProjectNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
