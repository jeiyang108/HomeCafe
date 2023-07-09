
import { IngredientService } from './../../../services/ingredient.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DrinkService } from '../../../services/drink.service';
import { TypeService } from './../../../services/type.service';
import { Drink } from '../../../models/drink.model';
import { Type } from '../../../models/type.model';
import { DatePipe } from '@angular/common';
import { DrinkIngredient } from '../../../models/drink-ingredient.model';


@Component({
  selector: 'app-edit-drink',
  templateUrl: './edit-drink.component.html',
  styleUrls: ['./edit-drink.component.css'],
  providers: [DatePipe]
})
export class EditDrinkComponent implements OnInit {
  selectedIngredient: DrinkIngredient = {
    ingredientId: 0,
    ingredientName: ''
  };
  updateDrinkRequest: Drink = {
    id: 0,
    name: '',
    description: '',
    dateCreated: new Date(),
    formattedDateCreated: '',
    isActive: true,
    image: '',
    types: [],
    drinkIngredients: []
  };
  types: Array<Type> = [];
  ingredientList: Array<DrinkIngredient> = [];
  file?: Blob;


  constructor(private route: ActivatedRoute, private drinkService: DrinkService,
    private typeService: TypeService, private ingredientService: IngredientService,
    private router: Router, private datePipe: DatePipe) { }

  ngOnInit(): void {
    // To grab id of the current drink record
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');

        if (id) {
          // Get details of the drink
          this.drinkService.getDrink(id)
            .subscribe({
              next: (response) => {
                this.updateDrinkRequest = response;
                this.updateDrinkRequest.formattedDateCreated = this.datePipe.transform(response.dateCreated, 'MMMM d, y h:mm a') ?? 'Undefined';
                this.updateDrinkRequest.image = 'data:image/png;base64,' + response.image;
              }
          });

          // Get all available drink types
          this.typeService.getTypes()
            .subscribe({
              next: (response) => {
                this.types = response;
              }
          });
          // Get all available ingredients
          this.ingredientService.getIngredients()
            .subscribe({
              next: (response) => {
                this.ingredientList = response;
              }
          });
        }

      }
    });
  }

  // Update the drink record when the form is submitted
  updateDrink() {
    this.drinkService.updateDrink(this.updateDrinkRequest.id.toString(), this.updateDrinkRequest)
      .subscribe({
        next: (response) => {
          this.router.navigate(['drinks']);
        }
      });
  }

  // Delete the drink record
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

  confirmImageUpdate() {
    if (confirm("Are you sure to update the image?")) {

      if (this.file == undefined) {
        alert("Please select an image.")

      } else {
        const formData = new FormData();
        formData.append('file', this.file);
        this.drinkService.updateDrinkPhoto(this.updateDrinkRequest.id.toString(), formData)
        .subscribe({
          next: (response) => {
            this.router.navigate(['drinks']);
          }
        });
      }
    }
  }

  onChangeFile(event: any) {
    // When image file is selected, load the file so it can be ready for confirmImageUpdate()
    if (event.target.files.length > 0) {
      this.file = event.target.files[0];
    }
  }

  hasType(object: Type) {
    // if the drink has the specified type, return true. Otherwise, false.
    return this.updateDrinkRequest.types?.findIndex(e => e.name == object.name) != -1;
  }

  updateType(item: Type) {
    let index = this.updateDrinkRequest.types?.findIndex(e => e.name == item.name);
    // if the updated type was already associated to the drink, remove the type. Otherwise, add the new type to the drink.
    if (index != -1) {
      this.updateDrinkRequest.types = this.updateDrinkRequest.types?.filter(e => e.name !== item.name);
    } else {
      this.updateDrinkRequest.types?.push(item);
    }
  }

  // Add the selected ingredient to the ingredient list at the bottom
  addSelectedIngredient(ingredient: DrinkIngredient) {
    let ingredientRef = this.ingredientList.find(i => i.ingredientName == ingredient.ingredientName);
    if (ingredientRef != undefined)
    {
      this.updateDrinkRequest.drinkIngredients?.push(ingredientRef);
      this.selectedIngredient.ingredientName = '';
    }
  }

  // Remove the ingredient from the list
  removeIngredient(ingredient: DrinkIngredient) {
    this.updateDrinkRequest.drinkIngredients = this.updateDrinkRequest.drinkIngredients?.filter(i => i.ingredientName !== ingredient.ingredientName);
  }

  // Set the amount to given value when the input field is updated
  setAmount(ingredient: DrinkIngredient, event: any) {
    ingredient.amount = parseInt(event.target.value);
  }
}


