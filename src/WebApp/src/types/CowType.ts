import { Advice } from "./Advice";

export interface Cow {
  id: number;
  name: string;
  lactationId: number;
  lactationName: string;
  days: number;
  milk: number;
  fat: number;
  protein: number;
  total: number;
  rv: number;
  advices: Advice[];
}


