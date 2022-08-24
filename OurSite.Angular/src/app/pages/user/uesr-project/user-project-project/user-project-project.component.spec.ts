import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProjectProjectComponent } from './user-project-project.component';

describe('UserProjectProjectComponent', () => {
  let component: UserProjectProjectComponent;
  let fixture: ComponentFixture<UserProjectProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserProjectProjectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserProjectProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
