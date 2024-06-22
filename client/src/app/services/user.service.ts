import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../environments/environment';
import { User } from '../models/user/user';
import { UpdateUser } from '../models/user/update-user';
import { UserFilter } from '../models/user-filter';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private pictureSource = new BehaviorSubject<string>('');
  private filteredUsersSubject = new BehaviorSubject<User[]>([]);
  filteredUsers$ = this.filteredUsersSubject.asObservable();


  picture$ = this.pictureSource.asObservable();
  
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getUsers(filters: UserFilter = {}): Observable<User[]> {
    const apiUrl = `${this.baseUrl}/User`;
    let httpParams = new HttpParams();
    if(filters.isActivatedFilter===0 || filters.isActivatedFilter===1)
      httpParams = httpParams.set("IsActivatedFilter",filters.isActivatedFilter);
    if(filters.searchUser)
      httpParams = httpParams.set("SearchUser",filters.searchUser);
    if(filters.roleFilter)
    {
      filters.roleFilter.forEach(element => {
        if(!httpParams.has("RoleFilter"))
        {
          httpParams = httpParams.set("RoleFilter",element);
        }
        else
          httpParams = httpParams.append("RoleFilter",element);
      });
    }
    // console.log(apiUrl);
    return this.http.get<User[]>(apiUrl, { params: httpParams });
  }

  getProjectUsers(project_id : number) : Observable<User[]>
  {
    const url = this.baseUrl + "/User/getProjectUsers/" + project_id;
    return this.http.get<User[]>(url);
  }
  getUserById(user_id : number) : Observable<User>
  {
    const url = this.baseUrl + "/User/getUserById/" + user_id;
    return this.http.get<User>(url);
  }

  getAssignmentUsers(asign_id:number) : Observable<User[]>
  {
    const url = this.baseUrl + "/User/getAssignmentUsers/"+asign_id;
    return  this.http.get<User[]>(url);
  }

  updateUser(user: UpdateUser) : Observable<User>
  {
    const url = this.baseUrl + "/User/updateUserInfo/"+user.id;
    const body = {
      user_id : user.id,
      name : user.name,
      lastname : user.lastname,
      username : user.username,
      jobTitle : user.jobTitle,
      organisation : user.organisation,
      department: user.department,
      roleId: user.roleId,
      isActivated: user.isActivated
    }
    return this.http.put<User>(url,body);
  }

  uploadProfilePicture(userId: number, picture: any): Observable<string> {
    const url = `${this.baseUrl}/User/${userId}/uploadProfilePicture`;
    //const headers = new HttpHeaders().set('Content-Type','application/json');
    return this.http.post<string>(url, picture);
  }
  
  profilePicture(userId: number): Observable<any> {
    const url = `${this.baseUrl}/User/${userId}/profilePicture`;
    return this.http.get<string>(url);
  }

  setProfilePicture(picture: string) {
    this.pictureSource.next(picture);
  }

  updateFilteredUsers(users: User[]) {
    this.filteredUsersSubject.next(users);
  }
}
