import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Item } from '../models/item.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiServiceService {
  private url = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  private getHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    });
  }

  getItemMaster(): Observable<Item[]> {
    const headers = this.getHeaders();
    return this.http.get<any>(`${this.url}api/auth/users`, { headers }).pipe(
      map((res) => res.payload),
      map((payload) => {
        return payload.map((item: any) => ({
          // itemid: item.itemid,
          // itemname: item.itemname,
          // parent: item.parent
        }));
      })
    );
  }

  getStocks(itemid: number): Observable<any> {
    const headers = this.getHeaders();
    const reqBody = { itemid: itemid };
    return this.http.post<any>(`${this.url}api/satcom`, reqBody, { headers });
  }

  getSaleDetails(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>(`${this.url}api/satcom/2`,{ headers } );
  }
}
