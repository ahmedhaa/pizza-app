import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth/services/auth.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {

    constructor(private authService: AuthService, private router: Router) { }

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {

        // Vérifier si l'utilisateur est authentifié et s'il est admin
        if (this.authService.isAuthenticated() && this.authService.isAdmin()) {
            console.log('User is authenticated and is an admin');
            return true;
        } else if (!this.authService.isAdmin()) {
            // Si l'utilisateur est un utilisateur standard, permettre l'accès à la page utilisateur
            return true;
        }
    

    // Si l'utilisateur n'est ni admin ni authentifié, rediriger vers la page de connexion
    this.router.navigate(['/login']);
return false;
    }

}