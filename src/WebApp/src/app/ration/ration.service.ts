import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LivestockProperties, NutrientType, Ration, UpdateRationCommand } from '../../types';

@Injectable({
  providedIn: 'root',
})
export class RationService {
  private apiUrl = environment.rationAPI;

  constructor(private http: HttpClient) {}

  private basalRationsSubject = new BehaviorSubject<any[]>([]);
  basalRations$ = this.basalRationsSubject.asObservable();

  getFeedTypeKeys(id: number, isEnergy: boolean): Observable<any> {
    return this.http.get<string[]>(
      `${this.apiUrl}/${id}/FeedTypeKeys?IsEnergy=${isEnergy}`
    );
  }

  removeFeedType(
    rationId: number,
    feedTypeId: number,
    isEnergy: boolean
  ): Observable<void> {
    const body = { feedTypeId, isEnergy };

    return this.http.put<void>(
      `${this.apiUrl}/${rationId}/RemoveFeedType`,
      body
    );
  }

  getRationById(id: number, isEnergy: boolean): Observable<Ration> {
    return this.http.get<Ration>(
      `${this.apiUrl}/${id ?? 0}?isEnergy=${isEnergy}`
    );
  }

  // getRationFeedTypes(id: number, isEnergy: boolean): Observable<any[]> {
  //   return this.http.get<any[]>(
  //     `${this.apiUrl}Ration/${id ?? 0}?isEnergy=${isEnergy}`
  //   );
  // }

  addFeedTypesToRation(
    rationId: number,
    feedTypes: any[],
    isEnergy: boolean
  ): Observable<any> {
    const url = `${this.apiUrl}/${rationId}/AddFeedType`;
    const body = {
      FeedTypes: feedTypes,
      IsEnergy: isEnergy,
    };

    return this.http.post<any>(url, body);
  }

  updateRationFeedType(
    rationId: number,
    command: UpdateRationCommand
  ): Observable<any> {
    return this.http.put(`${this.apiUrl}/${rationId}`, command);
  }

  removeEnergyFood(id: number, basalRationId: number): Observable<any> {
    console.log(
      `API Call to: ${this.apiUrl}/RemoveBasalRation/${id}?BasalFoodId=${basalRationId}`
    );

    return this.http.put<any>(
      `${this.apiUrl}/${id}/RemoveFoodType?BasalRationId=${basalRationId}`,
      {}
    );
  }

  GetLivestockProperty(id: number): Observable<LivestockProperties> {
    return this.http.get<LivestockProperties>(
      `${this.apiUrl}/${id ?? 0}/GetLivestockProperty`
    );
  }

  updateLivestockProperties(
    rationId: number,
    command: any
  ): Observable<any> {
    return this.http.put(`${this.apiUrl}/livestockProperties/${rationId}`, command);
  }

  createNewRation(rationName: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, { name: rationName });
  }
}
