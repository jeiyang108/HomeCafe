import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DrinkIngredient } from 'src/app/models/drink-ingredient.model';
import { IngredientService } from 'src/app/services/ingredient.service';
import { Status } from 'src/app/models/status.enum';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Unit } from 'src/app/models/unit.model'
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EditIngredientComponent } from '../edit-ingredient/edit-ingredient.component';
@Component({
  selector: 'app-ingredient-list',
  templateUrl: './ingredient-list.component.html',
  styleUrls: ['./ingredient-list.component.css']
})
export class IngredientListComponent implements OnInit {
  columns: string[] = ['ingredientId', 'ingredientName', 'unitName', 'ingredientStatus'];
  dataSource!: MatTableDataSource<DrinkIngredient>;
  ingredients: DrinkIngredient[] = [];
  // Data for Edit Component
  unitOptions!: Unit[];
  // MatPaginator Output
  pageEvent!: PageEvent;
  pageSize = 5;
  /* ViewChild points to a child element in the view DOM. Can access all public properties(values) and methods of the component.*/
  @ViewChild('paginator') paginator!: MatPaginator;
  // Available units


  constructor(private ingredientService: IngredientService, public dialog: MatDialog) { }

  ngOnInit(): void {
    // Get list of ingredients
    this.ingredientService.getIngredients()
      .subscribe({
        next: (ingredients) => {
          // Retrieve all ingredients that aren't deleted
          this.ingredients = ingredients.filter(x => x.status != Status.Deleted.toString());
          this.ingredients.forEach(i => i.isActive = (i.status == Status[Status.Active]));

          // Angular Material for table pagination
          this.dataSource = new MatTableDataSource<DrinkIngredient>(this.ingredients);
          this.dataSource.paginator = this.paginator;
          this.dataSource.filteredData = this.dataSource.data.slice(0, this.pageSize);

          //get units
          this.ingredientService.getUnits()
          .subscribe({
            next: (response) => {
              this.unitOptions = response;
            }
          });
        }
      });

  }
  // For pagination
  onPageChanged(event: any) {
    let firstCut = event.pageIndex * event.pageSize;
    let secondCut = firstCut + event.pageSize;
    this.dataSource.filteredData = this.dataSource.data.slice(firstCut, secondCut);
  }

  openEditIngredient(ingredient: DrinkIngredient) {
    // Create a deep copy so the table content won't be updated based on temporary changes made in the diaglog
    const ingredientCopy: DrinkIngredient = JSON.parse(JSON.stringify(ingredient));

    // Open the edit form
    const dialogRef = this.dialog.open(EditIngredientComponent, {
      data: {unitOptions:this.unitOptions, ingredient:ingredientCopy}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }
}



