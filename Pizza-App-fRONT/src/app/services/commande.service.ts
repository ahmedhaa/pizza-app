import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CommandeService {
  private apiUrl = `${environment.apiUrlp}client/commandes`;

  constructor(private http: HttpClient) { }

  getAllCommandes(pageNumber: number, pageSize: number): Observable<any> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<any>(`${this.apiUrl}`, { params });
  }

  updateCommandeStatus(id: number, statusUpdate: { status: string }) {
    console.log(statusUpdate)
    return this.http.put(`https://localhost:7265/${id}/status`, statusUpdate, {
      headers: { 'Content-Type': 'application/json' }
    });
  }
}
