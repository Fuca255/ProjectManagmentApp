import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../environments/environment';
import { State } from '../models/state/state';

@Injectable({
  providedIn: 'root'
})
export class StateService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  fetchStateName(stateId: number): Observable<string> {
    const apiUrl = `${this.apiUrl}/State/getById${stateId}`;
    return this.http.get<any>(apiUrl).pipe(
      map(response => response.name) // Extracts the name from the response
    );
  }

  fetchAllStates() : Observable<State[]>
  {
    const apiUrl = `${this.apiUrl}/State/getAll`;
    return this.http.get<State[]>(apiUrl);
  }
}
