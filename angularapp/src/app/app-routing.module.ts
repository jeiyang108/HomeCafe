import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DrinkListComponent } from './components/drinks/drink-list/drink-list.component';
import { AddDrinkComponent } from './components/drinks/add-drink/add-drink.component';
import { EditDrinkComponent } from './components/drinks/edit-drink/edit-drink.component';
import { IngredientListComponent } from './components/ingredients/ingredient-list/ingredient-list.component';

const routes: Routes = [

  {
    path: 'drinks',
    component: DrinkListComponent
  },
  {
    path: 'drinks/add',
    component: AddDrinkComponent
  },
  {
    path: 'drinks/edit/:id',
    component: EditDrinkComponent
  },
  {
    path: 'ingredients',
    component: IngredientListComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
