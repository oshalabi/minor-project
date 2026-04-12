import { NormCompare } from "./NormCompare";

export interface ToCompareCow {
    id: number;
    lactationId: number;
    totals: NormCompare[];
  }