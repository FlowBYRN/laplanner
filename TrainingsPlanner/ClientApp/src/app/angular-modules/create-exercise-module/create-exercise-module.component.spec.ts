import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateExerciseModuleComponent } from './create-exercise-module.component';

describe('CreateExerciseModuleComponent', () => {
  let component: CreateExerciseModuleComponent;
  let fixture: ComponentFixture<CreateExerciseModuleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateExerciseModuleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateExerciseModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
