import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, firstValueFrom, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  Advice,
  ComparedTotalRow,
  MappedTotals,
  NutrientType,
  TableSections,
  ToCompareCow,
} from '../../types';
import { BasalRationService } from '../basalration/basalration.service';
import { Cow } from '../../types/CowType';
import { NormCompare } from '../../types/NormCompare';

@Injectable({
  providedIn: 'root',
})
export class CowsOverviewService {
  private rationApiUrl = environment.rationAPI;
  private normApiUrl = environment.normAPI;

  constructor(
    private http: HttpClient,
    private basalRationService: BasalRationService
  ) {}

  private cowOverviewSubject = new BehaviorSubject<any[]>([]);
  cowOverview$ = this.cowOverviewSubject.asObservable();
  private cowOverviewAdvicesSubject = new BehaviorSubject<Advice[]>([
    {
      field: '',
      value: 0,
    },
  ]);
  cowOverviewAdvices$ = this.cowOverviewAdvicesSubject.asObservable();

  setcowOverviewAdvices(advices: Advice[]): void {
    this.cowOverviewAdvicesSubject.next(advices);
  }
  getcows(): Observable<any> {
    return this.http.get<any>(`${this.rationApiUrl}/cows`);
  }

  getCowGrouping(rationId: number): Observable<any> {
    const url = `${this.rationApiUrl}/${rationId}/CowsPerGroup`;
    return this.http.get(url);
  }
  compareNormsCowGrouping(data: ComparedTotalRow[]): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.post(`${this.normApiUrl}/CompareNorms`, data, { headers });
  }

  compareNormsAllCow(data: ToCompareCow[]): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.post(`${this.normApiUrl}/CompareNormsAllCows`, data, {
      headers,
    });
  }

  async getCowsData(
    callback: (data: Cow[]) => Promise<boolean>
  ): Promise<void> {
    this.getcows().subscribe({
      next: async (data: Cow[]) => {
        if (!data || data.length === 0) {
          console.warn('No cows data found.');
          return;
        }
        await callback(data);
      },
      error: (err) => {
        console.error('Error fetching cows data:', err);
      },
    });
  }

  async initializeNutrients(cowsSections: TableSections[]): Promise<void> {
    const nutrients = await this.basalRationService
      .getAllNutrientTypes()
      .subscribe({
        next: (data) => {
          const nutrientSection = cowsSections.find(
            (section) => section.header === 'Nutrienten'
          );
          if (nutrientSection) {
            nutrientSection.columns = data.map((nutrient: NutrientType) => ({
              field: nutrient.code,
              displayName: nutrient.code,
            }));
          }
        },
      });
  }

  async calculateNutrientsBasalValues(
    row: any,
    basalRationTotals: any
  ): Promise<MappedTotals> {
    const { total, advice1, advice2, advice3, advice4 } = row;
    const advice = advice1 + advice2 + advice3 + advice4;
    const basal = total - advice;

    try {
      if (!basalRationTotals?.values?.length) {
        return { values: [] };
      }

      return {
        values: basalRationTotals.values.map((nutrient: any) => ({
          field: nutrient.field,
          value: Number(nutrient.value) * basal,
        })),
      };
    } catch (error) {
      throw error;
    }
  }

  async energyFoodsValues(
    row: any,
    energyFoodValues: any
  ): Promise<MappedTotals> {
    const nutrientsEnergyFoodValues: MappedTotals = { values: [] };
    
    energyFoodValues.forEach((adviceEntry: any) => {
      const adviceValue = row[`advice${adviceEntry.advice}`] || 0;

      if (adviceValue > 0) {
        adviceEntry.nutrientsValues.forEach((nutrient: any) => {
          let existingNutrient = nutrientsEnergyFoodValues.values.find(
            (n) => n.field === nutrient.field && n.feedTypeId === row.feedTypeId
          );

          if (!existingNutrient) {
            existingNutrient = {
              field: nutrient.field,
              value: 0,
              feedTypeId: row.feedTypeId,
            };
            nutrientsEnergyFoodValues.values.push(existingNutrient);
          }

          existingNutrient.value =
            (Number(existingNutrient.value) || 0) +
            adviceValue * nutrient.value;
        });
      }
    });

    return nutrientsEnergyFoodValues;
  }
  async mapCowGroupingToCompare(data: any): Promise<ComparedTotalRow[]> {
    if (!Array.isArray(data) || data.length === 0) {
      console.error('Invalid data: Input is not an array or is empty.');
      return Promise.reject(new Error('Input data is not valid.'));
    }

    return data.map((row: any) => {
      if (!Array.isArray(row.totals) || row.totals.length === 0) {
        console.error('Invalid data: Input is not an array or is empty.');
      }

      return {
        totalCows: row.totalCows || 0,
        name: row.name || '',
        days: Number(row.days || 0),
        milk: Number(row.milk || 0),
        fat: Number(row.fat || 0),
        protein: Number(row.protein || 0),
        rv: Number(row.rv || 0),
        total: Number(row.total || 0),
        advices: [
          { field: 'advice1', value: row.advice1 || 0 },
          { field: 'advice2', value: row.advice2 || 0 },
          { field: 'advice3', value: row.advice3 || 0 },
          { field: 'advice4', value: row.advice4 || 0 },
        ],
        totals: row.totals.map((total: NormCompare) => ({
          field: total.field || '',
          value: Number(total.value || 0),
        })),
        group: row.group,
      };
    });
  }

  async mapCowsToCompare(data: any): Promise<ToCompareCow[]> {
    if (!Array.isArray(data) || data.length === 0) {
      console.error('Invalid data: Input is not an array or is empty.');
      return Promise.reject(new Error('Input data is not valid.'));
    }

    return data.map((row: any) => {
      if (!Array.isArray(row.totals) || row.totals.length === 0) {
        console.error('Invalid data: Input is not an array or is empty.');
      }

      return {
        id: row.id || 0,
        lactationId: row.lactationId || 0,
        totals: row.totals.map((total: NormCompare) => ({
          field: total.field || '',
          value: Number(total.value || 0),
        })),
      };
    });
  }

  isDataSourceNotEmpty(dataSource: any): boolean {
    return Array.isArray(dataSource) && dataSource.length > 0;
  }
}
