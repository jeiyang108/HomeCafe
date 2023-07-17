export enum Status {
  /* Ingredient Status */
  Active = 5, // Default / Available
  Inactive = 6, // Unavailable at the moment
  Deleted = 7 // Hidden from the list
}


export const statusOptions = [
  {value: Status.Active, viewValue: 'Available'},
  {value: Status.Inactive, viewValue: 'Unavailable'},
  {value: Status.Deleted, viewValue: 'Deleted'},
];

