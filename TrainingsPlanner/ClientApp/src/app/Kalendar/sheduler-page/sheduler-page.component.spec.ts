import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShedulerPageComponent } from './sheduler-page.component';

describe('ShedulerPageComponent', () => {
  let component: ShedulerPageComponent;
  let fixture: ComponentFixture<ShedulerPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShedulerPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShedulerPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
