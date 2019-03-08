import { HttpClient } from '@angular/common/http';
import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { map } from 'rxjs/operators';

import { User } from './user';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: object
  ) {
    if (isPlatformBrowser(this.platformId)) {
      this.localStorage = window.localStorage;
    }
  }

  private localStorage: any;

  public isAuthenticated(): boolean {
    return this.retrieveUserString() != null;
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
        if (this.localStorage) {
          this.localStorage.setItem('currentUser', JSON.stringify(user));
        }
      }
    }));
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
    return this.retrieveUserString();
  }

  private retrieveUserString(): any {
    if (this.localStorage) {
      return JSON.parse(this.localStorage.getItem('currentUser'));
    }
  }

}
