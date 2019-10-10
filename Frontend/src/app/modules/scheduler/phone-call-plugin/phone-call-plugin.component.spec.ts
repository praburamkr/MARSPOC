import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhoneCallPluginComponent } from './phone-call-plugin.component';

describe('PhoneCallPluginComponent', () => {
  let component: PhoneCallPluginComponent;
  let fixture: ComponentFixture<PhoneCallPluginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PhoneCallPluginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhoneCallPluginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
