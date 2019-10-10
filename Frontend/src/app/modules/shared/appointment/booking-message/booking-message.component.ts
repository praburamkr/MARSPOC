import { Component, OnInit, Input } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { IAppState } from 'src/app/core/store/state/app.state';
import { selectMiscData } from 'src/app/core/store/selector/misc/misc.selector';

@Component({
  selector: 'app-booking-message',
  templateUrl: './booking-message.component.html',
  styleUrls: ['./booking-message.component.scss']
})
export class BookingMessageComponent implements OnInit {
  @Input() noOfAppointments: number;
  $misc = this._store.pipe(select(selectMiscData));
  // tslint:disable-next-line: variable-name
  constructor(private _store: Store<IAppState>) { }


  ngOnInit() {
  }

}
