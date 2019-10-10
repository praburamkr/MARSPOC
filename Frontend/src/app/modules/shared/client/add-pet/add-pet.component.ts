import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-add-pet',
  templateUrl: './add-pet.component.html',
  styleUrls: ['./add-pet.component.scss']
})
export class AddPetComponent implements OnInit {

  constructor() { }
  @ViewChild('addPatientForm', { static: false }) addPatientForm: NgForm;
  // tslint:disable-next-line: no-output-on-prefix
  @Output() onAddPatient: EventEmitter<any> = new EventEmitter<any>();
  @Output() onClose: EventEmitter<any> = new EventEmitter<any>();
  ngOnInit() {
  }

  addPatient() {
    const newPatient = {
      patient_name: this.addPatientForm.value.patient_name,
      color_pattern: this.addPatientForm.value.color_pattern,
      age: +this.addPatientForm.value.age,
      weight: +this.addPatientForm.value.weight,
      species_name: this.addPatientForm.value.species_name,
      breed: this.addPatientForm.value.breed,
      sex: this.addPatientForm.value.gender,
      photo: 'assets/images/default_patient.png'
    };
    // console.log(newPatient);
    this.onAddPatient.emit(newPatient);
    this.addPatientForm.reset();
  }

  closeAddPet() {
    this.onClose.emit(null);
    this.addPatientForm.reset();
  }

}
