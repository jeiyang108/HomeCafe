import { Component, OnInit } from '@angular/core';
import { Drink } from '../../../models/drink.model';
import { DrinkService } from '../../../services/drink.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-drink',
  templateUrl: './add-drink.component.html',
  styleUrls: ['./add-drink.component.css']
})
export class AddDrinkComponent implements OnInit {

  addDrinkRequest: Drink = {
    id: 0,
    name: '',
    description: '',
    isActive: true,
    dateCreated: new Date(),
    image: ''
  };

  ngOnInit(): void {
  }
  constructor(private drinkService: DrinkService, private router: Router) { }

  addDrink() {
    this.drinkService.addDrink(this.addDrinkRequest)
      .subscribe({
        next: (drink) => {
          this.router.navigate(['drinks']);
        }
      });
  }
}
