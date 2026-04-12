import { Advice } from "./Advice";
import { NormCompare } from "./NormCompare";

export interface ComparedTotalRow {
    totalCows: number;
    name: string;
    days: number;
    milk: number;
    fat: number;
    protein: number;
    advices: Advice[];
    rv: number;
    total: number;
    totals: NormCompare[];
    group: number;

}

