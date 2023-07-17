import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Drink } from '../../../models/drink.model';
import { DrinkService } from '../../../services/drink.service';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-drink-list',
  templateUrl: './drink-list.component.html',
  styleUrls: ['./drink-list.component.css']
})


export class DrinkListComponent implements OnInit {
  drinks: Drink[] = [];
  columns: string[] = ['id', 'name', 'description', 'isActive', 'edit'];
  dataSource!: MatTableDataSource<Drink>;

  // MatPaginator Output
  pageEvent!: PageEvent;
  pageSize = 5;
  /* ViewChild: a local component template*/
  @ViewChild('paginator') paginator!: MatPaginator;

  constructor(private drinkService: DrinkService, private http: HttpClient,
    private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    //Get list of drinks
    this.drinkService.getAllDrinks()
      .subscribe({
        next: (drinks) => {
          // For public view (Display active records only)
          this.drinks = drinks.filter(x => x.isActive == true);
          this.drinks.forEach(drink => {
            drink.image = 'data:image/png;base64,' + drink.image;
          });

          // Angular Material (for admin section table)
          this.dataSource = new MatTableDataSource<Drink>(drinks);
          this.dataSource.paginator = this.paginator;
          this.dataSource.filteredData = this.dataSource.data.slice(0, this.pageSize);
        },
        error: (response) => {
          console.log(response);
        }
      });
  }

  onPageChanged(event: any) {
    let firstCut = event.pageIndex * event.pageSize;
    let secondCut = firstCut + event.pageSize;
    this.dataSource.filteredData = this.dataSource.data.slice(firstCut, secondCut);
  }

}
