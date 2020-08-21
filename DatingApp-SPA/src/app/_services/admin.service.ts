import { User } from './../_models/user';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getUsersWithRoles() {
  return this.http.get(this.baseUrl + 'admin/userWithRoles');
}

updateUserRoles(user: User, roles: {}) {
  return this.http.post(this.baseUrl + 'admin/editRoles/' + user.username, roles);
}

}
