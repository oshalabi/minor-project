import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class EnergyFeedSettingsService {
  private baseUrl = `${environment.rationAPI}`;

  constructor(private http: HttpClient) {}

  getEnergyFeedSettings(rationId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/${rationId}/EnergyFeedSettings?`);
  }

  setEnergyFeedSettings(feed: any): Observable<void> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
  
    return this.http.post<void>(`${this.baseUrl}/SetEnergyFeedSettings`, feed, { headers });
  }
}
