import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-phone-call-plugin',
  templateUrl: './phone-call-plugin.component.html',
  styleUrls: ['./phone-call-plugin.component.scss']
})
export class PhoneCallPluginComponent {
  client: any = {
    name: 'Brad',
    email: 'brad@microsoft.com',
    phone: '+010 98908898989',
    time: '05:30'
  }

  @Output() onAcceptPhoneCall: EventEmitter<any> = new EventEmitter<any>();
  @Input() clientInfo: Object;
  constructor() { }

  click() {

    var screen1 = document.getElementById('screen1');
    //  var screen2 = document.getElementById('screen2');
    screen1.style.display = 'none';
    //  screen2.style.display = 'block';
    //this.showClientSidebar = true;
  }

  clickAcceptPhoneCall() {
    this.onAcceptPhoneCall.emit();
  }

  closePhoneModal() {
    document.getElementById('screen2').style.display = 'none';
  }
}
