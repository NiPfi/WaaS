import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

import { User } from './user';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private router: Router) { }

  public isAuthenticated(): boolean {
    return this.retrieveUserString() != null;
  }

  public getToken(): string {
    return this.parseUser().token;
  }

  public getUserEmail(): string {
    return this.parseUser().email;
  }

  login(loginUser: User) {
    return this.http.post<User>(`${environment.apiUrl}/users/authenticate`, loginUser).pipe(map(user => {
      if (user && user.token) {
        localStorage.setItem('currentUser', JSON.stringify(user));
      }
    }));
  }

  register(registerUser: User) {
    return this.http.post<User>(`${environment.apiUrl}/users`, registerUser);
  }

  logout() {
    localStorage.removeItem('currentUser');
    this.router.navigate(['']);
  }

  private parseUser(): User {
    return this.retrieveUserString();
  }

  private retrieveUserString(): any {
    return JSON.parse(localStorage.getItem('currentUser'));
  }

}
