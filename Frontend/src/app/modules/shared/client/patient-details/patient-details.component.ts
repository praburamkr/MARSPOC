import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-patient-details',
  templateUrl: './patient-details.component.html',
  styleUrls: ['./patient-details.component.scss']
})
export class PatientDetailsComponent implements OnInit {
  clientPatients: any = [
    { 
      id:'patient100',
      name:'Lilly Cat',
      details : 
      {
        age:3
      }
    },
    { 
      id:'patient101',
      name:'Lebra Dog',
      details : 
      {
        age:3
      }
    }
  ];
  constructor() { }

  ngOnInit() {
  }
  onSave($event,tabId) {
    var patientAnchorTags = document.querySelectorAll('.patient-tab-details li a');    
    patientAnchorTags.forEach(function(eachTag) {
      //console.log(eachTag);  
      eachTag.classList.remove("patient-tab-selected");
    });
    document.getElementById('a-'+tabId).classList.add("patient-tab-selected");
  }
}
