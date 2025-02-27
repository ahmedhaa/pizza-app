import { PizzaDto } from "./model";

export interface PizzasResponse {
    items: PizzaDto[];     // Liste des pizzas
    pageNumber: number;    // Numéro de page
    pageSize: number;      // Taille de la page
    totalRecords: number;  // Nombre total d'éléments
    totalPages: number;    // Nombre total de pages
}