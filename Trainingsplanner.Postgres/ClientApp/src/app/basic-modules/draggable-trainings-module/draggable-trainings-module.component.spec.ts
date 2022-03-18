import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DraggableTrainingsModuleComponent } from './draggable-trainings-module.component';

describe('DraggableTrainingsModuleComponent', () => {
  let component: DraggableTrainingsModuleComponent;
  let fixture: ComponentFixture<DraggableTrainingsModuleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DraggableTrainingsModuleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DraggableTrainingsModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
