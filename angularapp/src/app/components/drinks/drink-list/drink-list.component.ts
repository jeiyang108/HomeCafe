import { Component, OnInit } from '@angular/core';
import { Drink } from '../../../models/drink.model';
import { DrinkService } from '../../../services/drink.service';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-drink-list',
  templateUrl: './drink-list.component.html',
  styleUrls: ['./drink-list.component.css']
})
export class DrinkListComponent implements OnInit {
  drinks: Drink[] = [];

  constructor(private drinkService: DrinkService, private http: HttpClient, private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    //Get list of drinks
    this.drinkService.getAllDrinks()
      .subscribe({
        next: (drinks) => {
          this.drinks = drinks;

          this.drinks.forEach(drink => {
            drink.image = 'data:image/png;base64,' + drink.image;
          });
        },
        error: (response) => {
          console.log(response);
        }
      });
  }

}
