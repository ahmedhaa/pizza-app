export interface PizzaDto {
  id: number;
  nom: string;
  prix: number;
  ingredients: { nom: string }[];
}