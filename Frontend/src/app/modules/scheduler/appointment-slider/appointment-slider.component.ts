import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-appointment-slider',
  templateUrl: './appointment-slider.component.html',
  styleUrls: ['./appointment-slider.component.scss']
})
export class AppointmentSliderComponent implements OnInit {

  constructor() { }
  @Input() clientId: number;
  @Input() clientInfo: Object;
  ngOnInit() {
  }

}
