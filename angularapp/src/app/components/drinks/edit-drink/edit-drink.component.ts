import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DrinkService } from '../../../services/drink.service';
import { Drink } from '../../../models/drink.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-edit-drink',
  templateUrl: './edit-drink.component.html',
  styleUrls: ['./edit-drink.component.css'],
  providers: [DatePipe]
})
export class EditDrinkComponent implements OnInit {

  updateDrinkRequest: Drink = {
    id: 0,
    name: '',
    description: '',
    dateCreated: new Date(),
    formattedDateCreated: '',
    isActive: true
  };

  constructor(private route: ActivatedRoute, private drinkService: DrinkService, private router: Router, private datePipe: DatePipe) { }

  ngOnInit(): void {
    // To grab id of the current drink record
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');

        if (id) {
          //call api
          this.drinkService.getDrink(id)
            .subscribe({
              next: (response) => {
                this.updateDrinkRequest = response;
                this.updateDrinkRequest.formattedDateCreated = this.datePipe.transform(response.dateCreated, 'MMMM d, y h:mm a');
              }
            });
        }

      }
    });
  }

  updateDrink() {
    this.drinkService.updateDrink(this.updateDrinkRequest.id.toString(), this.updateDrinkRequest)
      .subscribe({
        next: (response) => {
          this.router.navigate(['drinks']);
        }
      });
  }

  deleteDrink() {
    // Delete the drink, close the modal and redirect to the drink list page.
    this.drinkService.deleteDrink(this.updateDrinkRequest.id.toString())
      .subscribe({
        next: (response) => {
          document.getElementById("closeBtn")?.click();
          this.router.navigate(['drinks']);
        }
      });
  }

}
