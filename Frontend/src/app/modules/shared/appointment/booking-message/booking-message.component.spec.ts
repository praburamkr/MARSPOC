import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BookingMessageComponent } from './booking-message.component';

describe('BookingMessageComponent', () => {
  let component: BookingMessageComponent;
  let fixture: ComponentFixture<BookingMessageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookingMessageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookingMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
