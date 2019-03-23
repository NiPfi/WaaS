import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

import { AuthService } from '../authentication/auth.service';
import { User } from '../authentication/user';

@Injectable({
  providedIn: 'root'
})
export class EditProfileService {
  constructor(private http: HttpClient, private auth: AuthService) { }

  updateEmail(newEmail: string) {
    return this.http.put<User>(`${environment.apiUrl}/users`, {newEmail}).subscribe(data => {
      this.auth.updateUser(data);
    },
    error => {
      // TODO Handle error
    });
  }

  updatePassword(currentPassword: string, newPassword: string) {
    return this.http.put<User>(`${environment.apiUrl}/users`, {currentPassword, newPassword}).subscribe(data => {
      this.auth.updateUser(data);
    },
    error => {
      // TODO Handle error
    });
  }

  deleteAccount() {
    return this.http.delete(`${environment.apiUrl}/users`, { }).subscribe((data) => {
      this.auth.logout();
    },
    error => {
      // TODO Handle error
    });
  }
}
