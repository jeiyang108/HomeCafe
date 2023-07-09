import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { DrinkIngredient } from '../models/drink-ingredient.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IngredientService {
  baseApiUrl: string = environment.baseApiUrl;

  constructor(private http: HttpClient) { }

  getIngredients(): Observable<DrinkIngredient[]> {
    return this.http.get<DrinkIngredient[]>(this.baseApiUrl + '/api/ingredients');
  }
}
