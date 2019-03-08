import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthService } from '../authentication/auth.service';

@Injectable({
  providedIn: 'root'
})
export class EditProfileService {

  constructor(private http: HttpClient, private auth: AuthService) { }

  deleteAccount() {
    return this.http.delete(`${environment.apiUrl}/users`, { }).subscribe((data) => {
      this.auth.logout();
    },
    error => {
      // TODO Handle error
    });
  }
}
