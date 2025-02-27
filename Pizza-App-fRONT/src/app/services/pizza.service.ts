import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PizzasResponse } from '../pizza-management/interfaces/PizzasResponse';
import { PizzaCreateDto } from '../pizza-management/interfaces/PizzaCreateDto';


@Injectable({
  providedIn: 'root'
})
export class PizzaService {
  private apiUrl = `${environment.apiUrlp}admin/pizzas`;

  constructor(private http: HttpClient) { }

  getAllPizzas(pageNumber: number = 1, pageSize: number = 10): Observable<PizzasResponse> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<PizzasResponse>(this.apiUrl, { params });
  }
  // Récupérer tous les ingrédients
  getIngredients(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/ingredients`);// L'URL de l'API pour récupérer les ingrédients
  }
  // Méthode pour créer une nouvelle pizza
  createPizza(pizzaCreateDto: PizzaCreateDto): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      // Ajoutez ici d'autres en-têtes nécessaires comme le token d'authentification, par exemple:
      // 'Authorization': `Bearer ${localStorage.getItem('auth_token')}`
    });
    return this.http.post<any>(this.apiUrl, pizzaCreateDto, { headers });

}

  updatePizza(id: number, pizzaUpdateDto: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, pizzaUpdateDto);
  }

  // Méthode pour supprimer une pizza
  deletePizza(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
