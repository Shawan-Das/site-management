import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(this.apiUrl + 'api/auth/login', { login: username, pwd: password }).pipe(
      tap(response => {
        const token = response.payload.token;
        sessionStorage.setItem('token', token);
      })
    );
  }

  logout(): void {
    sessionStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!sessionStorage.getItem('token');
  }
}

