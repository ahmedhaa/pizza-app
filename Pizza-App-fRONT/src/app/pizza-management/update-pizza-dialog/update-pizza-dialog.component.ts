import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PizzaService } from '../../services/pizza.service'; // Service pour gérer les pizzas

@Component({
  selector: 'app-update-pizza-dialog',
  templateUrl: './update-pizza-dialog.component.html',
})
export class UpdatePizzaDialogComponent {
  pizza: any = {}; // Contiendra les informations de la pizza à mettre à jour
  ingredientsList: any[] = []; // Liste des ingrédients disponibles

  constructor(
    public dialogRef: MatDialogRef<UpdatePizzaDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private pizzaService: PizzaService
  ) {
    // Initialisation avec les données passées au dialogue
    this.pizza = { ...data.pizza }; // Pizza à modifier
    this.ingredientsList = data.ingredientsList; // Liste des ingrédients
    console.log('Données reçues dans le dialogue:', data); // Vérifier les données reçues

  }

  // Méthode pour mettre à jour la pizza
  updatePizza(): void {

    if (!this.pizza.id) {
      console.error('L\'ID de la pizza est manquant');
      return; // Empêche la mise à jour si l'ID est manquant
    }

    const pizzaUpdateDto = {
      id: this.pizza.id,
      Nom: this.pizza.nom,
      Prix: this.pizza.prix,
      IngredientsIds: this.pizza.ingredientsIds,
    };

    this.pizzaService.updatePizza(this.pizza.id, pizzaUpdateDto).subscribe(
      updatedPizza => {
        // Fermer la boîte de dialogue avec la pizza mise à jour
        this.dialogRef.close(updatedPizza);
      },
      error => {
        console.error("Erreur lors de la mise à jour de la pizza", error);
      }
    );
  }
  closeDialog(): void {
    this.dialogRef.close();  // Ferme la fenêtre sans effectuer de mise à jour
  }
}
