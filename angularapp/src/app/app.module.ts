import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { DrinkListComponent } from './components/drinks/drink-list/drink-list.component';
import { AppRoutingModule } from './app-routing.module';
import { AddDrinkComponent } from './components/drinks/add-drink/add-drink.component';
import { FormsModule } from '@angular/forms';
import { EditDrinkComponent } from './components/drinks/edit-drink/edit-drink.component';
import { IngredientListComponent } from './components/ingredients/ingredient-list/ingredient-list.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { EditIngredientComponent } from './components/ingredients/edit-ingredient/edit-ingredient.component';
import { MatDialogModule } from '@angular/material/dialog';

@NgModule({
  declarations: [
    AppComponent,
    DrinkListComponent, //Standalone cannot be declared in an NgModule
    AddDrinkComponent,
    EditDrinkComponent,
    IngredientListComponent,
    EditIngredientComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSlideToggleModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDialogModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
