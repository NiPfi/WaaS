import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/internal/operators/catchError';
import { map } from 'rxjs/internal/operators/map';
import { HttpErrorHandlerService } from 'src/app/error-handling/http-error-handler.service';
import { environment } from 'src/environments/environment';

import { User } from '../user';

@Injectable({
  providedIn: 'root'
})
export class VerificationService {

  constructor(
    private readonly http: HttpClient,
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
}
