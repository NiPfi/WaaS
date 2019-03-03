import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TestService {

  constructor(private http: HttpClient) { }

  getTest() {
    return this.http.get(`${environment.apiUrl}/test`);
  }
}
