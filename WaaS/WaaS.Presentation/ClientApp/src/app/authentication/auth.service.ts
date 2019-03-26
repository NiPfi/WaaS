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
    private readonly cookies: CookieService,
  ) {
  }
  private readonly userKey = 'currentUser';
  private readonly cookieOptions: CookieOptions = {
    domain: environment.apiUrl,
    httpOnly: true,
    secure: true
  };

  public isAuthenticated(): boolean {
    return (this.parseUser() != null);
  }

  public getToken(): string {
    return this.parseUser().token;
  }

  public getUserEmail(): string {
    return this.parseUser().email;
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
    this.cookies.putObject(this.userKey, user);
  }

  logout() {
    this.cookies.removeAll();
    this.router.navigate(['']);
  }

  private parseUser(): User {
    const user = this.cookies.getObject(this.userKey) as User;

    if (user) {
      const expired = this.jwtHelper.isTokenExpired(user.token);
      return (expired) ? null : user;
    }
  }

}
