import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointmentSliderComponent } from './appointment-slider.component';

describe('AppointmentSliderComponent', () => {
  let component: AppointmentSliderComponent;
  let fixture: ComponentFixture<AppointmentSliderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppointmentSliderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppointmentSliderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
