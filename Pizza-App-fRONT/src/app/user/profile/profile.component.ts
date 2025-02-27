import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/services/auth.service';
import { PizzaDto } from 'src/app/pizza-management/interfaces/model';
import { PizzaService } from 'src/app/services/pizza.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  pizzas: PizzaDto[] = [];
  pageNumber: number = 1;
  pageSize: number = 10;
  currentUser: any = null;
  userName: string = ''; 
  constructor(private pizzaService: PizzaService,private authService: AuthService) { }

  ngOnInit() {
    this.authService.currentUser.subscribe(user => {
      this.currentUser = user;
      console.log(this.currentUser);

      if (this.currentUser) {
        this.userName = this.currentUser.name;  // Mise à jour de userName
      }

      // Vérifie si l'utilisateur est un admin
    });
    // Charger les pizzas au moment de l'initialisation du composant
    this.loadPizzas();
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
  logout(): void {
    this.authService.logout();
    // Rediriger l'utilisateur vers la page de login après la déconnexion
    // Vous pouvez utiliser le Router pour cela, par exemple :
    // this.router.navigate(['/login']);
  }
}
