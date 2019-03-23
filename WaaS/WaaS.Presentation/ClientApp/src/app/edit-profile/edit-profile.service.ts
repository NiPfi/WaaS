import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

import { AuthService } from '../authentication/auth.service';
import { User } from '../authentication/user';
import { HttpErrorHandlerService } from '../error-handling/http-error-handler.service';

@Injectable({
  providedIn: 'root'
})
export class EditProfileService {
  constructor(
    private readonly http: HttpClient,
    private readonly auth: AuthService,
    private readonly errorHandler: HttpErrorHandlerService
  ) { }

  updateEmail(newEmail: string) {
    return this.http.put<User>(`${environment.apiUrl}/users`, { newEmail })
      .pipe(catchError(this.errorHandler.handleError))
      .pipe(map(data => {
        this.auth.updateUser(data);
        return data;
      }));
  }

  updatePassword(currentPassword: string, newPassword: string) {
    return this.http.put<User>(`${environment.apiUrl}/users`, { currentPassword, newPassword })
      .pipe(catchError(this.errorHandler.handleError));
  }

  deleteAccount() {
    return this.http.delete(`${environment.apiUrl}/users`, {})
      .pipe(catchError(this.errorHandler.handleError))
      .pipe(map((data) => {
        this.auth.logout();
        return data;
      }));
  }
}
