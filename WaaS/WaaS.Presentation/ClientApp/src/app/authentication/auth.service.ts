import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

import { JwtHelperService } from './jwt/jwt-helper.service';
import { User } from './user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    private router: Router,
    private jwtHelper: JwtHelperService,
    @Inject(PLATFORM_ID) private platformId: object
  ) {
    if (isPlatformBrowser(this.platformId)) {
      this.localStorage = window.localStorage;
    }
  }

  private localStorage: any;

  public isAuthenticated(): boolean {
    return (this.parseUser() != null);
  }

  public getToken(): string {
    return this.parseUser().token;
  }

  public getUserEmail(): string {
    return this.parseUser().email;
  }

  login(loginUser: User, captchaResponse: string) {
    return this.http.post<User>(`${environment.apiUrl}/users/authenticate`, {
      user: loginUser,
      captchaResponse
    }
    ).pipe(map(user => {
      if (user && user.token) {
        this.updateUser(user);
      }
    }));
  }

  updateUser(user: User) {
    if (this.localStorage) {
      this.localStorage.setItem('currentUser', JSON.stringify(user));
    }
  }

  register(registerUser: User, captchaResponse: string) {
    return this.http.post<User>(`${environment.apiUrl}/users`, {
      user: registerUser,
      captchaResponse
    });
  }

  logout() {
    if (this.localStorage) {
      this.localStorage.removeItem('currentUser');
    }
    this.router.navigate(['']);
  }

  private parseUser(): User {

    if (this.localStorage) {
      const userString = JSON.parse(this.localStorage.getItem('currentUser'));
      const user = userString as User;
      if (user) {
        const expired = this.jwtHelper.isTokenExpired(user.token);
        return (expired) ? null : user;
      }
      return null;
    }

  }

}
