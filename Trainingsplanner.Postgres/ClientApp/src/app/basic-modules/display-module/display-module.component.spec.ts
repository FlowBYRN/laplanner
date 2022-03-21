import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplayModuleComponent } from './display-module.component';

describe('DisplayModuleComponent', () => {
  let component: DisplayModuleComponent;
  let fixture: ComponentFixture<DisplayModuleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DisplayModuleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DisplayModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
