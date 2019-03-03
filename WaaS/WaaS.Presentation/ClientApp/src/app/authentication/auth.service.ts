import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from 'rxjs/operators';

import { User } from './user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private jwtHelper: JwtHelperService, private http: HttpClient) { }

  public isAuthenticated(): boolean {
    return !this.jwtHelper.isTokenExpired(this.parseUser().token);
  }

  public get token(): any {
    return this.jwtHelper.decodeToken(this.parseUser().token);
  }

  login(loginUser: User) {
    return this.http.post<any>(`${config.apiUrl}/users/authenticate`, { loginUser }).pipe(map(user => {
      if (user && user.token) {
        localStorage.setItem('currentUser', JSON.stringify(user));
      }
    }))
  }

  private parseUser(): User {
    return JSON.parse(localStorage.getItem('currentUser'));
  }

}
