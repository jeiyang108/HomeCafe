import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { DrinkListComponent } from './components/drinks/drink-list/drink-list.component';
import { AppRoutingModule } from './app-routing.module';
import { AddDrinkComponent } from './components/drinks/add-drink/add-drink.component';
import { FormsModule } from '@angular/forms';
import { EditDrinkComponent } from './components/drinks/edit-drink/edit-drink.component';

@NgModule({
  declarations: [
    AppComponent,
    DrinkListComponent,
    AddDrinkComponent,
    EditDrinkComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
