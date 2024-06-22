import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { Task } from '../models/task/task';
import { TaskGroup } from '../models/task/task-group';
import { UpdateGant } from '../models/gantogram/update_gant_task';

@Injectable({
  providedIn: 'root'
})
export class GantogramService {

  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  private tasks = []


  GetTasksByProjectId(project_id:number) : Observable<Task[]>
  {
    const url = this.baseUrl+"/TaskGroup/getTaskGroupsByProjectWithTasks/"+project_id;
    return this.http.get<Task[]>(url);
  }


  GetAssignemntsByProjectId(project_id:number) : Observable<Task[]>
  {
    const url = this.baseUrl+"/Assignment/getAssignmentsByProject/"+project_id
    return this.http.get<Task[]>(url)
  }


  GetGroupByProjectId(project_id:number) : Observable<TaskGroup>
  {
    const url = this.baseUrl+"/TaskGroup/getTaskGroupsByProjectId/"+project_id;
    return this.http.get<TaskGroup>(url);
  }


  updateTaskById(body : UpdateGant, asign_id : number) : Observable<Task>
  {
    const url = this.baseUrl + "/Assignment/updateAssigmentGantt/" + asign_id;
    return this.http.put<Task>(url,body);
  }
}
