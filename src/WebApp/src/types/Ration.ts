import { RationFeedType } from './RationFeedType';

// Ration type
export interface Ration {
  id: number;
  name: string;
  rationTotalKg: number;
  rationTotalKgDs: number;
  dsProcentWeightedSum: number;
  averageTotalFeedTypes: Array<{ key: string; value: string |number }>

  feedTypes: RationFeedType[];
}
