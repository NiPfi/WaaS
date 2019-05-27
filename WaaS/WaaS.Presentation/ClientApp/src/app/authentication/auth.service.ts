import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { HttpErrorHandlerService } from '../error-handling/http-error-handler.service';
import { JwtHelperService } from './jwt/jwt-helper.service';
import { User } from './user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private readonly http: HttpClient,
    private readonly router: Router,
    private readonly jwtHelper: JwtHelperService,
    private readonly handler: HttpErrorHandlerService,
    @Inject(PLATFORM_ID) private readonly platformId: object
  ) {
    if (isPlatformBrowser(this.platformId)) {
      this.localStorage = window.localStorage;
      this.localStorageUserKey = 'currentUserToken';
    }
  }
  private readonly localStorage: any;
  private readonly localStorageUserKey: string;

  public isAuthenticated(): boolean {
    const token = this.getUserToken();
    if (token !== '') {
      return !this.jwtHelper.isTokenExpired(token);
    }
    return false;
  }

  public getUserEmail(): string {
    return this.jwtHelper.getTokenEmail(this.getUserToken());
  }

  register(registerUser: User, captchaResponse: string): Observable<{} | User> {
    return this.http.post<User>(`${environment.apiUrl}/users`, {
      user: registerUser,
      captchaResponse
    }).pipe(catchError(this.handler.handleError));
  }

  login(loginUser: User, captchaResponse: string): Observable<User> {
    return this.http.post<User>(`${environment.apiUrl}/users/authenticate`, {
      user: loginUser,
      captchaResponse
    }
    ).pipe(map(user => {
      if (user && user.token) {
        this.updateUser(user);
      }
      return user;
    }))
      .pipe(catchError(this.handler.handleError));
  }

  updateUser(user: User) {
    if (this.localStorage) {
      this.localStorage.setItem(this.localStorageUserKey, user.token);
    }
  }

  logout() {
    if (this.localStorage) {
      this.localStorage.removeItem(this.localStorageUserKey);
    }
    this.router.navigate(['']);
  }

  public getUserToken(): string {
    if (this.localStorage) {
      return this.localStorage.getItem(this.localStorageUserKey);
    }
    return null;

  }

}
