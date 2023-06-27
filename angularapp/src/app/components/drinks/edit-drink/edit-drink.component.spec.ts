import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDrinkComponent } from './edit-drink.component';

describe('EditDrinkComponent', () => {
  let component: EditDrinkComponent;
  let fixture: ComponentFixture<EditDrinkComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditDrinkComponent]
    });
    fixture = TestBed.createComponent(EditDrinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
