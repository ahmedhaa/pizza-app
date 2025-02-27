import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/services/auth.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  isAdmin: boolean = false;
  currentUser: any = null;
  userName: string = ''; 
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    // Souscrire à l'observable currentUser pour obtenir les informations de l'utilisateur connecté
    this.authService.currentUser.subscribe(user => {
      this.currentUser = user;
      console.log(this.currentUser);

      if (this.currentUser) {
        this.userName = this.currentUser.name;  // Mise à jour de userName
      }

      // Vérifie si l'utilisateur est un admin
      this.isAdmin = this.authService.isAdmin();
    });
  }
  logout(): void {
    this.authService.logout();
    // Rediriger l'utilisateur vers la page de login après la déconnexion
    // Vous pouvez utiliser le Router pour cela, par exemple :
    // this.router.navigate(['/login']);
  }
}
