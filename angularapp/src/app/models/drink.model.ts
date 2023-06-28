import { SafeUrl } from "@angular/platform-browser";

export interface Drink {
  id: number;
  name: string;
  description: string;
  dateCreated: Date;
  formattedDateCreated?: string | null;
  isActive: boolean;
  image: string;
  imageNew?: string;
}
