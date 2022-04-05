import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WeeklySheduleComponent } from './weekly-shedule.component';

describe('WeeklySheduleComponent', () => {
  let component: WeeklySheduleComponent;
  let fixture: ComponentFixture<WeeklySheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WeeklySheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WeeklySheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
