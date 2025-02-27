import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import jwt_decode from 'jwt-decode';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  // L'URL de l'API backend pour l'authentification
  private apiUrl = `https://localhost:7265/api/auth`;
  private tokenKey = 'auth_token';
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;

  constructor(private http: HttpClient, private router: Router) { 
    this.currentUserSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('currentUser') || '{}'));
    this.currentUser = this.currentUserSubject.asObservable();
  }
  // Fonction pour décoder le token et récupérer le rôle
  decodeToken(token: string): any {
   return jwt_decode(token);
  }
  // Fonction de connexion
  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { email, password }).pipe(
      tap(response => {
        if (response.token) {
          localStorage.setItem(this.tokenKey, response.token);  // Stockage du token
          const decodedToken = this.decodeToken(response.token); // Décodage du token pour obtenir les informations utilisateur
          this.setCurrentUser({ token: response.token }); // Passez l'objet correct à setCurrentUser
        } else {
          console.error('No token received from the server');
        }
       // console.log('Token reçu:', response.token);
        //console.log('Token stocké:', localStorage.getItem(this.tokenKey));

      })
    );
  }
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // Fonction d'inscription
  register(user: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  // Fonction pour récupérer les informations de l'utilisateur connecté
  getProfile(): Observable<any> {
    return this.http.get(`${this.apiUrl}/me`);
  }

  setCurrentUser(user: any): void {
    if (!user.token) {
      console.error('Token is missing');
      return; // Empêcher l'exécution si le token est absent
    }

    const decodedToken = this.decodeToken(user.token);  // Décodage du token
    const userName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];  // Extraire le nom
    const currentUser = { ...decodedToken, name: userName }; // Ajouter le nom à l'objet
    localStorage.setItem('currentUser', JSON.stringify(currentUser));
    this.currentUserSubject.next(currentUser);
  }

  // Fonction pour se déconnecter (supprimer le token du localStorage)
  logout(): void {
    localStorage.removeItem(this.tokenKey); // Supprimer le token du localStorage
    localStorage.removeItem('currentUser'); // Supprimer les informations utilisateur
    this.currentUserSubject.next(null); // Réinitialiser le BehaviorSubject

    // Rediriger vers la page de login après la déconnexion
    this.router.navigate(['/login']);
  }
  // Fonction pour vérifier si l'utilisateur est authentifié (vérifie si un token est présent dans le localStorage)
  isAuthenticated(): boolean {
    const token = localStorage.getItem(this.tokenKey);
    return token != null; 
  }
  // Vérification du rôle de l'utilisateur depuis le token stocké
  isAdmin(): boolean {
    const token = localStorage.getItem(this.tokenKey);
    if (token) {
      const decodedToken = this.decodeToken(token);
     

      const userRole = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      console.log(userRole)
      return userRole === 'Admin'; // Retourner true si le rôle est 'admin'
    }
    return false;
  }
}


