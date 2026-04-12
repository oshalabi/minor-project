import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { TopbarService } from '../topbar/topbar.service'; // Import TopbarService

@Injectable({
  providedIn: 'root',
})
export class GraphDataService {
  private apiUrl = `${environment.energyFoodAPI}EnergyFood/`;

  private rationIdSubject = new BehaviorSubject<number | null>(null);
  rationId$ = this.rationIdSubject.asObservable();

  constructor(
    private http: HttpClient,
    private topbarService: TopbarService
  ) {
    this.topbarService.getSelectedRation().subscribe((ration) => {
      if (ration) {
        this.rationIdSubject.next(ration.id);
      }
    });
  }

  getGraphData(
    energyFoodAmount: number,
    basalRationDVPAmount: number,
    basalRationVEMBasicAmount: number,
    livestockProperty: any
  ): Observable<{ melkproductieVEM: number; energyFoodAmount: number }> {
    const url = `${this.apiUrl}GetGraphInformation`;
    const body = {
      EnergyFoodAmount: energyFoodAmount,
      BasalRationDVPAmount: basalRationDVPAmount,
      BasalRationVEMBasicAmount: basalRationVEMBasicAmount,
      LivestockProperty: livestockProperty,
    };
    return this.http.post<{
      melkproductieVEM: number;
      energyFoodAmount: number;
    }>(url, body);
  }
}
