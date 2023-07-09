import { Component, OnInit } from '@angular/core';
import { Drink } from '../../../models/drink.model';
import { DrinkService } from '../../../services/drink.service';
import { Router } from '@angular/router';
import { DrinkIngredient } from '../../../models/drink-ingredient.model';
import { Type } from 'src/app/models/type.model';
import { TypeService } from 'src/app/services/type.service';
import { IngredientService } from 'src/app/services/ingredient.service';

@Component({
  selector: 'app-add-drink',
  templateUrl: './add-drink.component.html',
  styleUrls: ['./add-drink.component.css']
})
export class AddDrinkComponent implements OnInit {
  selectedIngredient: DrinkIngredient = {
    ingredientId: 0,
    ingredientName: ''
  };
  addDrinkRequest: Drink = {
    id: 0,
    name: '',
    description: '',
    isActive: true,
    dateCreated: new Date(),
    image: '',
    types: [],
    drinkIngredients: []
  };
  types: Array<Type> = [];
  ingredientList: Array<DrinkIngredient> = [];
  file?: Blob;

  constructor(private drinkService: DrinkService, private router: Router,
    private typeService: TypeService, private ingredientService: IngredientService) { }


  ngOnInit(): void {
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

  addDrink() {
    if (this.file == undefined) {
      alert("Please select an image.")
    } else {
      const formData = new FormData();
      // Append the file to the form data
      formData.append('file', this.file);
      // Convert the 'addDrinkRequest' object to JSON
      const addDrinkRequestJson = JSON.stringify(this.addDrinkRequest);
      // Append the JSON data as a separate field in the form data
      formData.append('addDrinkRequest', addDrinkRequestJson);
      // Here, you can make a HTTP request to send the image data to the backend server
      this.drinkService.addDrink(formData)
        .subscribe({
          next: (drink) => {
            this.router.navigate(['drinks']);
          }
      });
      //const imageFile = formData.get('file') as File;
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
    return this.addDrinkRequest.types?.findIndex(e => e.name == object.name) != -1;
  }

  updateType(item: Type) {
    let index = this.addDrinkRequest.types?.findIndex(e => e.name == item.name);
    // if the updated type was already associated to the drink, remove the type. Otherwise, add the new type to the drink.
    if (index != -1) {
      this.addDrinkRequest.types = this.addDrinkRequest.types?.filter(e => e.name !== item.name);
    } else {
      this.addDrinkRequest.types?.push(item);
    }
  }

  // Add the selected ingredient to the ingredient list at the bottom
  addSelectedIngredient(ingredient: DrinkIngredient) {
    let ingredientRef = this.ingredientList.find(i => i.ingredientName == ingredient.ingredientName);
    if (ingredientRef != undefined)
    {
      this.addDrinkRequest.drinkIngredients?.push(ingredientRef);
      this.selectedIngredient.ingredientName = '';
    }
  }

  // Remove the ingredient from the list
  removeIngredient(ingredient: DrinkIngredient) {
    this.addDrinkRequest.drinkIngredients = this.addDrinkRequest.drinkIngredients?.filter(i => i.ingredientName !== ingredient.ingredientName);
  }

  // Set the amount to given value when the input field is updated
  setAmount(ingredient: DrinkIngredient, event: any) {
    ingredient.amount = parseInt(event.target.value);
  }
}
