import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthService } from '../authentication/auth.service';
import { User } from '../authentication/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EditProfileService {
  constructor(private http: HttpClient, private auth: AuthService) { }

  update(userDto: User) {
    return this.http.put<User>(`${environment.apiUrl}/users`, userDto).subscribe(data => {
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
