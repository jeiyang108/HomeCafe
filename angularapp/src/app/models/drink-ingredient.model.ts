import { Unit } from "./unit.model";

export interface DrinkIngredient {
  ingredientId: number;
  name: string;
  status?: string;
  isActive?: boolean;
  amount?: number;
  unit: Unit;
}
