import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { ApiError } from './api-error';
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
    @Inject(PLATFORM_ID) private readonly platformId: object
  ) {
    if (isPlatformBrowser(this.platformId)) {
      this.localStorage = window.localStorage;
    }
  }

  private readonly localStorage: any;

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
    }).pipe(catchError(this.handleError));
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
      .pipe(catchError(this.handleError));
  }

  updateUser(user: User) {
    if (this.localStorage) {
      this.localStorage.setItem('currentUser', JSON.stringify(user));
    }
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
    }
    return null;

  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
      return throwError(`There was an error sending your request: ${error.error.message}`);
    } else {
      const apiError = error.error as ApiError;
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${apiError.statusCode}: ${apiError.statusDescription}, ` +
        `Message: ${apiError.message}`);

      if (error.message) {
        return throwError(apiError.message);
      } else {
        return throwError(`An error has occurred. Details have been logged to the console.`);
      }
    }
    // return an observable with a user-facing error message
  }

}
