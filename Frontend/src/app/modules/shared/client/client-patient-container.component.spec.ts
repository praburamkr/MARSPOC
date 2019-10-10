import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientPatientContainerComponent } from './client-patient-container.component';

describe('ClientPatientContainerComponent', () => {
  let component: ClientPatientContainerComponent;
  let fixture: ComponentFixture<ClientPatientContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientPatientContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientPatientContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
