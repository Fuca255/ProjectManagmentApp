import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Team } from '../models/team';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TeamService {

  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getTeams(): Observable<Team[]> {
    return this.http.get<Team[]>(`${this.apiUrl}/Team/getTeams`);
  }
  getMyCreatedTeams(userId : number, searchText: string = "") : Observable<Team[]>
  {
    const url = this.apiUrl + "/Team/getAllCreatedTeamsByUser/" + userId;
    let httpParams : HttpParams = new HttpParams();
    if(searchText!=="")
      httpParams = httpParams.set("searchText",searchText);
    return this.http.get<Team[]>(url,{params:httpParams});
  }

  createTeam(body: any) : Observable<Team>{
    const url = this.apiUrl + "/Team/createTeam";
    return this.http.post<Team>(url,body);
  }

  updateTeam(body: any, team_id:number) : Observable<Team>{
    const url = this.apiUrl + "/Team/update/" + team_id;
    return this.http.put<Team>(url,body);
  }
  deleteTeam(team_id: number)
  {
    const url = this.apiUrl + "/Team/delete/"+team_id;
    return this.http.delete(url);
  }
}
