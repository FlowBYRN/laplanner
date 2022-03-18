import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleplannerPageComponent } from './moduleplanner-page.component';

describe('ModuleplannerPageComponent', () => {
  let component: ModuleplannerPageComponent;
  let fixture: ComponentFixture<ModuleplannerPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleplannerPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleplannerPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
