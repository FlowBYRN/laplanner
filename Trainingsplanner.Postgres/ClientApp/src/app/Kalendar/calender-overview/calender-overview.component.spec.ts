import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalenderOverviewComponent } from './calender-overview.component';

describe('CalenderOverviewComponent', () => {
  let component: CalenderOverviewComponent;
  let fixture: ComponentFixture<CalenderOverviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalenderOverviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalenderOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
