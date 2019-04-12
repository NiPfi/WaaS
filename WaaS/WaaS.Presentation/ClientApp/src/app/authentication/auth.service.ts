import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CookieOptions, CookieService } from 'ngx-cookie';
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
    private readonly cookies: CookieService
  ) {
  }
  private readonly userKey = 'currentUserToken';
  private readonly cookieOptions: CookieOptions = {
    domain: environment.apiUrl,
    httpOnly: true,
    secure: true
  };

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

  logout() {
    this.cookies.removeAll();
    this.router.navigate(['']);
  }

  updateUser(user: User) {
    this.cookies.put(this.userKey, user.token);
  }

  public getUserToken(): string {
    const token = this.cookies.get(this.userKey);
    if (token) {
      return this.jwtHelper.isTokenExpired(token) ? '' : token;
    }
  }

}
