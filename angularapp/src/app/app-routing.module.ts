import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DrinkListComponent } from './components/drinks/drink-list/drink-list.component';
import { AddDrinkComponent } from './components/drinks/add-drink/add-drink.component';
import { EditDrinkComponent } from './components/drinks/edit-drink/edit-drink.component';

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
