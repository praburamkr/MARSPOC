// import { Component, OnInit } from '@angular/core';

// @Component({
//   selector: 'app-upload-file',
//   templateUrl: './upload-file.component.html',
//   styleUrls: ['./upload-file.component.scss']
// })
// export class UploadFileComponent implements OnInit {

//   constructor() { }

//   ngOnInit() {
//   }

// }
import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
 
@Component({
  // selector: 'app-image-upload-with-preview',
  // templateUrl: './image-upload-with-preview.component.html',
  // styleUrls: ['./image-upload-with-preview.component.css']
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.scss']
})
 
export class UploadFileComponent implements OnInit {
 
  fileData: File = null;
  previewUrl:any = null;
  fileUploadProgress: string = null;
  uploadedFilePath: string = null;
  constructor(private http: HttpClient) { }
   
  ngOnInit() {
  }
   
  fileProgress(fileInput: any) {
      this.fileData = <File>fileInput.target.files[0];
      this.preview();
  }
 
  preview() {
    // Show preview 
    var mimeType = this.fileData.type;
    if (mimeType.match(/image\/*/) == null) {
      return;
    }
 
    var reader = new FileReader();      
    reader.readAsDataURL(this.fileData); 
    reader.onload = (_event) => { 
      this.previewUrl = reader.result; 
    }
  }
   
  // onSubmit() {
  //     const formData = new FormData();
  //     formData.append('file', this.fileData);
  //     this.http.post('url/to/your/api', formData)
  //       .subscribe(res => {
  //         console.log(res);
  //         this.uploadedFilePath = res.data.filePath;
  //         alert('SUCCESS !!');
  //       }) 
  // }

  onSubmit() {
    const formData = new FormData();
    formData.append('files', this.fileData);
     
    this.fileUploadProgress = '0%';
 
    this.http.post('https://us-central1-tutorial-e6ea7.cloudfunctions.net/fileUpload', formData, {
      reportProgress: true,
      observe: 'events'   
    })
    .subscribe(events => {
      if(events.type === HttpEventType.UploadProgress) {
        this.fileUploadProgress = Math.round(events.loaded / events.total * 100) + '%';
        console.log(this.fileUploadProgress);
      } else if(events.type === HttpEventType.Response) {
        this.fileUploadProgress = '';
        console.log(events.body);          
        alert('SUCCESS !!');
      }
         
    }) 
}
}