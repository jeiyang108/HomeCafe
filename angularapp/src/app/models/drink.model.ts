import { DrinkIngredient } from "./drink-ingredient.model";
import { Type } from "./type.model";

export interface Drink {
  id: number;
  name: string;
  description: string;
  dateCreated: Date;
  formattedDateCreated?: string;
  isActive: boolean;
  image: string;
  types?: Array<Type>;
  drinkIngredients?: Array<DrinkIngredient>;
}


