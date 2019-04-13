import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/internal/operators/catchError';
import { map } from 'rxjs/internal/operators/map';
import { HttpErrorHandlerService } from 'src/app/error-handling/http-error-handler.service';
import { environment } from 'src/environments/environment';

import { AuthService } from '../auth.service';
import { User } from '../user';

@Injectable({
  providedIn: 'root'
})
export class VerificationService {

  constructor(
    private readonly http: HttpClient,
    private readonly auth: AuthService,
    private readonly handler: HttpErrorHandlerService
  ) { }

  verifyEmailConfirmation(email: string, token: string): Observable<User> {
    return this.http.post(`${environment.apiUrl}/users/verify`, {
      email,
      verificationToken: token
    }).pipe(map(user => {
      return user as User;
    })).pipe(catchError(this.handler.handleError));
  }

  resendConfirmationEmail(email: string, captchaResponse: string): Observable<object> {
    return this.http.post(`${environment.apiUrl}/users/resend-confirmation-email`, { user: { email }, captchaResponse }
    ).pipe(catchError(this.handler.handleError));
  }

  verifyEmailChange(email: string, token: string): Observable<User> {
    return this.http.post(`${environment.apiUrl}/users/verify-mail-change`, {
      email,
      verificationToken: token
    }).pipe(map(user => {
      this.auth.updateUser(user as User);
      return user as User;
    })).pipe(catchError(this.handler.handleError));
  }
}
