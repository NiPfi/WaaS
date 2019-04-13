import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

import { User } from '../authentication/user';
import { HttpErrorHandlerService } from '../error-handling/http-error-handler.service';

@Injectable({
  providedIn: 'root'
})
export class EditProfileService {
  constructor(
    private readonly http: HttpClient,
    private readonly errorHandler: HttpErrorHandlerService
  ) { }

  updateEmail(newEmail: string) {
    return this.http.put<User>(`${environment.apiUrl}/users`, { newEmail })
      .pipe(catchError(this.errorHandler.handleError));
  }

  updatePassword(currentPassword: string, newPassword: string) {
    return this.http.put<User>(`${environment.apiUrl}/users`, { currentPassword, newPassword })
      .pipe(catchError(this.errorHandler.handleError));
  }

  deleteAccount() {
    return this.http.delete(`${environment.apiUrl}/users`, {})
      .pipe(catchError(this.errorHandler.handleError));
  }
}
