import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleBrowseComponent } from './module-browse.component';

describe('ModuleBrowseComponent', () => {
  let component: ModuleBrowseComponent;
  let fixture: ComponentFixture<ModuleBrowseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleBrowseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleBrowseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
