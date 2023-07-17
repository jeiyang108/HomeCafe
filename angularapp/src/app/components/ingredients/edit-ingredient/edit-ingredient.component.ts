import { IngredientService } from 'src/app/services/ingredient.service';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { DrinkIngredient } from 'src/app/models/drink-ingredient.model';
import { Status } from 'src/app/models/status.enum';
import { Router } from '@angular/router';
import { Unit } from 'src/app/models/unit.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-edit-ingredient',
  templateUrl: './edit-ingredient.component.html',
  styleUrls: ['./edit-ingredient.component.css']
})
export class EditIngredientComponent {
  @Input()
  ingredient!: DrinkIngredient;
  unitOptions!: Unit[];

  constructor(private ingredientService: IngredientService, private router: Router, public dialogRef: MatDialogRef<EditIngredientComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.ingredient = data.ingredient;
    this.unitOptions = data.unitOptions;
  }


  closeDialog(): void {
    // Close the dialog without saving changes
    this.dialogRef.close();
  }

  deleteIngredient() {
    // Delete the drink, close the modal and redirect to the drink list page.
    this.ingredientService.deleteIngredient(this.ingredient.ingredientId.toString())
      .subscribe({
        next: (response) => {
          document.getElementById("closeBtn")?.click();
          this.router.navigate(['drinks']);
        }
      });
  }

  // Compare the mat-option list and mat-select's value
  isCurrentUnitValue(option: Unit, value: Unit): boolean {
    return option.name == (value ? value.name: value);
  }

  updateIngredient() {
    // Update the status value based on the toggle state.
    this.ingredient.status = this.ingredient.isActive ? Status[Status.Active] : Status[Status.Inactive];
    this.ingredientService.updateIngredient(this.ingredient)
      .subscribe({
        next: (response) => {
          this.dialogRef.close(/* pass any result or data you want to return */);
          window.location.reload();
        }
      });
  }

}
