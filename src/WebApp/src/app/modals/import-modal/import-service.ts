import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ImportType } from '../../../types';

@Injectable({
  providedIn: 'root',
})
export class ImportService {

  private urls = {
    [ImportType.FEEDTYPE]: `${environment.importRation}/upload`,
    [ImportType.NORM]: `${environment.normAPI}/upload`,
    [ImportType.COWS]: `${environment.rationAPI}/`,
  };
  constructor(private http: HttpClient) {}

  uploadFile(file: File, type: ImportType, rationId: number): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    let url = this.urls[type];

    if (type === ImportType.COWS)
      url += `${rationId}/upload`;

    return this.http.post(url, formData);
  }
}

