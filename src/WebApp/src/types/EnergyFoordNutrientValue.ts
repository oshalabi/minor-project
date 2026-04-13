export interface EnergyFoordNutrientValue {
    id?: number;
    advice: number;
    nutrientsValues: Array<{ field: string; value: string |number }>
}