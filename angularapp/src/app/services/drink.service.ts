import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Drink} from '../models/drink.model';

@Injectable({
  providedIn: 'root'
})
export class DrinkService {

  baseApiUrl: string = environment.baseApiUrl;
  constructor(private http: HttpClient) { }

  getAllDrinks(): Observable<Drink[]> {
    return this.http.get<Drink[]>(this.baseApiUrl + '/api/drink');
  }

  addDrink(formData: FormData): Observable<Drink> {
    return this.http.post<Drink>(this.baseApiUrl + '/api/drink', formData);
  }

  getDrink(id: string): Observable<Drink> {
    return this.http.get<Drink>(this.baseApiUrl + '/api/drink/' + id);
  }

  updateDrink(id: string, updateDrinkRequest: Drink): Observable<Drink> {
    return this.http.put<Drink>(this.baseApiUrl + '/api/drink/' + id, updateDrinkRequest);
  }

  updateDrinkPhoto(id: string, formData: FormData): Observable<Drink> {
    return this.http.put<Drink>(this.baseApiUrl + '/api/drink/photo/' + id, formData);
  }

  deleteDrink(id: string): Observable<Drink> {
    return this.http.delete<Drink>(this.baseApiUrl + '/api/drink/' + id);
  }


}
