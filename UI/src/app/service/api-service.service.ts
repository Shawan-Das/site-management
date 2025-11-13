import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Satcom, SatcomResponse } from '../models/satcom.model';
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

  // Create Satcom Data
  createSatcom(data: Satcom): Observable<SatcomResponse> {
    const headers = this.getHeaders();
    return this.http.post<SatcomResponse>(`${this.url}api/satcom`, data, { headers });
  }

  // Get All Satcom Data
  getCompanyList(): Observable<SatcomResponse> {
    const headers = this.getHeaders();
    return this.http.get<SatcomResponse>(`${this.url}api/satcom`, { headers });
  }

  // Get One Satcom Data
  getOneData(id: number): Observable<SatcomResponse> {
    const headers = this.getHeaders();
    return this.http.get<SatcomResponse>(`${this.url}api/satcom/${id}`, { headers });
  }

  // Update Satcom Data
  updateSatcom(id: number, data: Satcom): Observable<SatcomResponse> {
    const headers = this.getHeaders();
    return this.http.put<SatcomResponse>(`${this.url}api/satcom/${id}`, data, { headers });
  }

  // Delete Satcom Data
  deleteSatcom(id: number): Observable<SatcomResponse> {
    const headers = this.getHeaders();
    return this.http.delete<SatcomResponse>(`${this.url}api/satcom/${id}`, { headers });
  }
}
