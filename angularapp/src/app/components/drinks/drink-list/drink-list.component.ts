import { Component, OnInit } from '@angular/core';
import { Drink } from '../../../models/drink.model';
import { DrinkService } from '../../../services/drink.service';

@Component({
  selector: 'app-drink-list',
  templateUrl: './drink-list.component.html',
  styleUrls: ['./drink-list.component.css']
})
export class DrinkListComponent implements OnInit {
  drinks: Drink[] = [];
  constructor(private drinkService: DrinkService) { }
  ngOnInit(): void {
    //Get list of drinks
    this.drinkService.getAllDrinks()
      .subscribe({
        next: (drinks) => {
          this.drinks = drinks;
        },
        error: (response) => {
          console.log(response);
        }
      });
  }
}
