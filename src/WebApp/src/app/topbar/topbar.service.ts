import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Ration } from './topbar.component';

@Injectable({
  providedIn: 'root',
})
export class TopbarService {
  private apiUrl = environment.rationAPI;

  constructor(private http: HttpClient) {}

  @Output() fetchDataRequested = new EventEmitter<void>();

  onFetchDataClick(): void {
    this.fetchDataRequested.emit(); // Emit het event
  }

  private selectedRationSubject = new BehaviorSubject<Ration | null>(null);

  setSelectedRation(ration: Ration): void {
    this.selectedRationSubject.next(ration);
  }

  getSelectedRation() {
    return this.selectedRationSubject.asObservable();
  }

  getRations(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}`);
  }

  getRation(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }
}
