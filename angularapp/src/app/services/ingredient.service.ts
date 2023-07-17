import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { DrinkIngredient } from '../models/drink-ingredient.model';
import { Observable } from 'rxjs';
import { statusOptions } from '../models/status.enum';
import { Unit } from '../models/unit.model';

@Injectable({
  providedIn: 'root'
})
export class IngredientService {
  updateIngredient(ingredient: DrinkIngredient) {
    return this.http.put<DrinkIngredient>(this.baseApiUrl + '/api/ingredients', ingredient);
  }

  updateIngredientStatus(id: number, action: string) {
    return this.http.put<DrinkIngredient>(this.baseApiUrl + '/api/ingredients/' + id, action);
  }

  baseApiUrl: string = environment.baseApiUrl;
  constructor(private http: HttpClient) { }

  getIngredients(): Observable<DrinkIngredient[]> {
    return this.http.get<DrinkIngredient[]>(this.baseApiUrl + '/api/ingredients');
  }

  deleteIngredient(id: string): Observable<DrinkIngredient> {
    return this.http.delete<DrinkIngredient>(this.baseApiUrl + '/api/ingredients/' + id);
  }

  getUnits(): Observable<Unit[]> {
    return this.http.get<Unit[]>(this.baseApiUrl + '/api/units');
  }

  /*
  getUnitViewValue(unitId: number): string {
    const unitOption = unitOptions.find(option => option.value === unitId);
    return unitOption ? unitOption.viewValue : '';
  }

  getStatusValue(statusId: number): string {
    const statusOption = statusOptions.find(option => option.value === statusId);
    return statusOption ? statusOption.viewValue : '';
  }
  */

}
