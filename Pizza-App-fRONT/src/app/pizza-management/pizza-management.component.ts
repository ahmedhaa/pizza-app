import { Component, OnInit } from '@angular/core';
import { PizzaDto } from './interfaces/model';
import { PizzaService } from '../services/pizza.service';
import { PizzaCreateDto } from './interfaces/PizzaCreateDto';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { UpdatePizzaDialogComponent } from './update-pizza-dialog/update-pizza-dialog.component';

@Component({
  selector: 'app-pizza-management',
  templateUrl: './pizza-management.component.html',
  styleUrls: ['./pizza-management.component.scss']
})
export class PizzaManagementComponent implements OnInit {
  // pizzas: any[] = []; 
  newPizza: PizzaCreateDto = {
    nom: '',
    prix: 0,
    ingredientsIds: []
  };

  ingredientsList: any[] = []; // Liste des ingrédients à afficher dans le formulaire
  message: string = '';
  pizzas: PizzaDto[] = [];
  pageNumber: number = 1;
  pageSize: number = 10;
  constructor(private pizzaService: PizzaService, private snackBar: MatSnackBar, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadPizzas();
    this.loadIngredients();

  }

  openUpdateDialog(pizza: any): void {
    // Vérifiez si l'ID de la pizza est défini
    console.log(pizza)
    if (!pizza.id) {
      console.error('L\'ID de la pizza est manquant');
      return; // Ne pas ouvrir le dialogue si l'ID est manquant
    }

    // Ouvrir le dialogue si l'ID est présent
    const dialogRef = this.dialog.open(UpdatePizzaDialogComponent, {
      width: '400px',
      data: {
        pizza: pizza, // La pizza à modifier
        ingredientsList: this.ingredientsList // Liste des ingrédients
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadPizzas();
        this.showToast('Pizza mise à jour avec succee');
        console.log('Pizza mise à jour:', result);
      }
    });
  }
  loadIngredients(): void {
    this.pizzaService.getIngredients().subscribe(ingredients => {
      this.ingredientsList = ingredients;
      console.log(this.ingredientsList)// Stocker les ingrédients dans le tableau
    });
  }
  // Fonction pour afficher un toast
  private showToast(message: string, type: string = 'error'): void {
    this.snackBar.open(message, 'Fermer', {
      duration: 3000,
      panelClass: type === 'error' ? 'error-snackbar' : 'success-snackbar',
    });
  }
  // Méthode pour soumettre le formulaire et créer la pizza
  createPizza(): void {
    if (!this.newPizza.nom) {
      this.showToast('Le nom de la pizza est requis.');
      return;
    }

    if (this.newPizza.prix <= 0) {
      this.showToast('Le prix doit être supérieur à 0.');
      return;
    }

    if (this.newPizza.ingredientsIds.length === 0) {
      this.showToast('Veuillez sélectionner au moins un ingrédient.');
      return;
    }
    this.pizzaService.createPizza(this.newPizza).subscribe(response => {
      this.showToast('Pizza ajoutée avec succès!', 'success');
      this.loadPizzas(); // Recharger la liste des pizzas après l'ajout
      this.newPizza = { nom: '', prix: 0, ingredientsIds: [] }; // Réinitialiser le formulaire
    }, error => {
      this.showToast("Erreur lors de la création de la pizza.", 'error');
      console.error("Erreur lors de la création de la pizza:", error);
    });
  }
  loadPizzas(): void {

    this.pizzaService.getAllPizzas(this.pageNumber, this.pageSize).subscribe(
      (data) => {
        this.pizzas = data.items;
        console.log(data)
      },
      (error) => {
        console.error('Erreur lors de la récupération des pizzas:', error);
      }
    );
  }
  onIngredientsChange(selectedIngredients: any): void {
    console.log(selectedIngredients);
    this.newPizza.ingredientsIds = selectedIngredients;
  }

  // Méthode pour supprimer une pizza
  deletePizza(id: number): void {
    if (confirm('Êtes-vous sûr de vouloir supprimer cette pizza ?')) {
      this.pizzaService.deletePizza(id).subscribe(
        () => {
          // Après la suppression, retirez la pizza de la liste locale
          this.pizzas = this.pizzas.filter(pizza => pizza.id !== id);
          console.log('Pizza supprimée avec succès');
          this.showToast('Pizza supprimée avec succès');

        },
        error => {
          console.error('Erreur lors de la suppression de la pizza', error);
          this.showToast('Erreur lors de la suppression de la pizza');

        }
      );
    }
  }

}
