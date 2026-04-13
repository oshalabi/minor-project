import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  NutrientType,
  Ration,
  UpdateRationCommand,
  NutrientTypeValueEnums,
  FlattenedRationFeedType,
  EnergyFoordNutrientValue,
  FeedType,
  RationFeedType,
} from '../../types';
import { RationService } from '../ration/ration.service';

@Injectable({
  providedIn: 'root',
})
export class EnergyFoodService {

  private apiUrl = environment.energyFoodAPI;

  constructor(
    private http: HttpClient,
    private rationService: RationService
  ) {}


  private energyFoodsSubject = new BehaviorSubject<any[]>([]);
  energyFoodsValues$ = this.energyFoodsSubject.asObservable();
  setEnergyFoodsNutrientValues(energyFoods: EnergyFoordNutrientValue[]) {
    this.energyFoodsSubject.next(energyFoods); 
  }
  GetAvailableFeedTypes(
    categoryValues: number[],
    feedTypeKeys: string[]
  ): Observable<any> {
    const body = {
      CategoryValues: categoryValues,
      FeedTypeKeys: feedTypeKeys,
    };

    return this.http.post<any>(
      `${this.apiUrl}/AvailableFeedTypes`,
      body
    );
  }

  getAllNutrientTypes(): Observable<NutrientType[]> {
    return this.http.get<NutrientType[]>(
      `${this.apiUrl}/GetAllNutrients`
    );
  }

  updateEnergyFoods(updatedRations: any): void {
    const energyFoodsArray = updatedRations.energyFoods;

    if (energyFoodsArray && energyFoodsArray.length > 0) {
      this.energyFoodsSubject.next(energyFoodsArray); // Push updated data to observable
    } else {
      console.warn('No valid basal rations data to update.');
    }
  }

  mergeNutrientLists(
    nutrients: { field: string; displayName: string }[],
    rationEnergyFoodTypeNutrients: { field: string; displayName: string }[]
  ): { field: string; displayName: string }[] {
    const nutrientsColumns: Record<
      string,
      { field: string; displayName: string }
    > = {};
    const excludedNutrients = this.getExcludedNutrients();

    nutrients.forEach((nutrient) => {
      nutrientsColumns[nutrient.field] = nutrient;
    });

    rationEnergyFoodTypeNutrients.forEach((nutrient) => {
      if (!excludedNutrients.includes(nutrient.field)) {
        nutrientsColumns[nutrient.field] = nutrient;
      }
    });

    return Object.values(nutrientsColumns);
  }

  getExcludedNutrients(): string[] {
    return [
      NutrientTypeValueEnums.Vem,
      NutrientTypeValueEnums.Dvp,
      NutrientTypeValueEnums.Oep,
      NutrientTypeValueEnums.Bzb,
    ];
  }

  mapNutrientsToColumns(
    nutrients: { code: string; value: number }[]
  ): Record<string, number> {
    const nutrientMap: Record<string, number> = {};
    nutrients.forEach((nutrient) => {
      nutrientMap[nutrient.code] = nutrient.value;
    });
    return nutrientMap;
  }

  roundToInteger(value: number): number {
    return Math.round(value);
  }

  roundToTwoDecimals(value: number): number {
    return Math.round(value * 100) / 100;
  }

  calculateNutritionalValue(kg: number, baseValue: number): number {
    return this.roundToInteger(this.roundToTwoDecimals(kg * baseValue));
  }

  async updateRationData(
    rationId: number,
    callback: (data: Ration) => Promise<boolean>
  ): Promise<void> {
    if (!rationId) {
      console.warn('Invalid ration ID provided.');
      return Promise.reject('Invalid ration ID.');
    }

    this.rationService.getRationById(rationId, true).subscribe({
      next: async (data: Ration) => {
        if (!data) {
          console.warn('No data found for the provided ration ID.');
          return Promise.reject('No data found.');
        }

        try {
          await callback(data);
        } catch (error) {
          console.error('Error processing basal rations:', error);
        }
      },
      error: (err) => {
        console.error('Error fetching basal rations by ID:', err);
      },
    });
  }

  updateRationRow = async (
    command: UpdateRationCommand,
    rationId: number,
    callback: (data: Ration) => Promise<boolean>
  ): Promise<void> => {
    if (!command || !command.feedTypeId ) {
      // Handle invalid row data
      console.error('Invalid row data:', command);
      return Promise.reject('Invalid row data.');
    }
    // Call the service to update the ration
    this.rationService.updateRationFeedType(rationId, command).subscribe({
      next: () => {
        this.updateRationData(rationId, callback);
        return Promise.resolve();
      },
      error: (err) => {
        console.error('Error updating ration:', err);
        setTimeout(() => {}, 500);
      },
    });
  };

  async loadAllNutrientsTypes(): Promise<
    { field: string; displayName: string }[]
  > {
    try {
      const data = await this.getAllNutrientTypes().toPromise();

      if (!data) return [];

      const excludedNutrients = this.getExcludedNutrients();

      // Map and filter the nutrients
      const nutrients = data
        .map((nutrient) => ({
          field: nutrient.code,
          displayName: nutrient.code,
        }))
        .filter(
          (nutrient) =>
            !excludedNutrients.includes(
              nutrient.field as NutrientTypeValueEnums
            )
        );

      return nutrients;
    } catch (error) {
      console.error('Error fetching nutrients:', error);
      throw error;
    }
  }

  updateNutritionalValues(row: any, callback: () => void): void {
    // Nutrient columns specific to "Gemiddelde opname per dier per kg"
    const nutrientColumns = [
      NutrientTypeValueEnums.Vem,
      NutrientTypeValueEnums.Dvp,
      NutrientTypeValueEnums.Oep,
    ];

    nutrientColumns.forEach((column) => {
      const baseValue = row[column];
      if (baseValue !== undefined) {
        row[column.toUpperCase()] = this.calculateNutritionalValue(
          row.kg,
          baseValue
        );
      } else {
        console.warn(`Value for column ${column} not found in row`, row);
      }
    });
    callback();
  }

  async recalculateRow(
    row: any,
    field: string,
    callback: () => void  ): Promise<boolean> {
    try {
      const dsProcent = row.dsProcent || 0;

      if (field.toLowerCase() === 'kg') {
        if (row.kg < 0) {
          throw new Error('KG value must be greater than or equal to 0.');
        }
        row.kgDs = this.roundToTwoDecimals(row.kg * (dsProcent / 100));
      } else if (field.toLowerCase() === 'kgds') {
        if (row.kgDs < 0) {
          throw new Error('KG DS value must be greater than or equal to 0.');
        }
        row.kg = this.roundToTwoDecimals(row.kgDs / (dsProcent / 100));
      }

      // Clear error fields if recalculation succeeds
      row.errorFields = {};
      this.updateNutritionalValues(row, callback);
      return true; // Return true if successful
    } catch (error: any) {
      console.error('Error recalculating row:', error);
      throw error; // Propagate the error
    }
  }

  removeEnergyFoodFeedType(
    rationId: number,
    feedTypeId: number,
    isEnergy: boolean
  ): Observable<void> {
    return this.rationService.removeFeedType(rationId, feedTypeId, isEnergy);
  }
}
