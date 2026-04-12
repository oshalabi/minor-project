import { Category } from './Category';
import { Nutrient } from './Nutrient';

// FeedType type
export interface FeedType {
  nutrientsValuesPerKgDs: any;
  id: number;
  code: number;
  name: string;
  dsProcent: number;
  category?: Category;
  nutrients: Nutrient[];
}
