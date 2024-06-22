import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { TaskGroup } from '../models/task/task-group';
import { environment } from '../environments/environment';
import { TaskFilter } from '../models/task/task-filter';


@Injectable({
  providedIn: 'root'
})
export class TaskGroupService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  constructor() { }

  getAllProjectTaskGroups(project_id:number) : Observable<TaskGroup[]>
  {
    const url = this.baseUrl + "/TaskGroup/getTaskGroupsByProjectId/"+project_id;
    return this.http.get<TaskGroup[]>(url);
  }
  getAllProjectTaskGroupsWithAssignments(project_id:number,params: TaskFilter = {}): Observable<TaskGroup>
  {
    const url = this.baseUrl+"/TaskGroup/getTaskGroupsByProjectWithTasks/" + project_id;

    let httpParams = new HttpParams();
    
    if (params.propertyName) {
      httpParams = httpParams.set('PropertyName', params.propertyName);
    }
    if (params.sortFlag) {
      httpParams = httpParams.set('SortFlag', params.sortFlag.toString());
    }
    if (params.pageNumber) {
      httpParams = httpParams.set('PageNumber', params.pageNumber.toString());
    }
    if (params.pageSize) {
      httpParams = httpParams.set('PageSize', params.pageSize.toString());
    }
    if (params.searchTitle) {
      httpParams = httpParams.set('SearchTitle', params.searchTitle);
    }
    if (params.startFrom) {
      httpParams = httpParams.set('StartFrom', params.startFrom.toDateString());
    }
    if (params.startTo) {
      httpParams = httpParams.set('StartTo', params.startTo.toDateString());
    }
    if (params.endFrom) {
      httpParams = httpParams.set('EndFrom', params.endFrom.toDateString());
    }
    if (params.endTo) {
      httpParams = httpParams.set('EndTo', params.endTo.toDateString());
    }
    if (params.stateFilter) {
      params.stateFilter.forEach(element => {
        if(!httpParams.has("StateFilter"))
          httpParams = httpParams.set('StateFilter',element.toString());
        else
          httpParams = httpParams.append("StateFilter",element.toString());
      });
    }
    if (params.user_ids) {
      params.user_ids.forEach(element => {
        if(!httpParams.has("user_ids"))
          httpParams = httpParams.set('user_ids',element.toString());
        else
          httpParams = httpParams.append("user_ids",element.toString());
      });
    }
    if (params.percentageFilterFrom) {
      httpParams = httpParams.set('PercentageFilterFrom', params.percentageFilterFrom.toString());
    }
    if (params.percentageFilterTo) {
      httpParams = httpParams.set('PercentageFilterTo', params.percentageFilterTo.toString());
    }
    if (params.priorityFilter) {
      params.priorityFilter.forEach(element => {
        if(!httpParams.has("PriorityFilter"))
          httpParams = httpParams.set('PriorityFilter', element.toString());
        else
          httpParams = httpParams.append("PriorityFilter",element.toString());
      });
    }

    return this.http.get<TaskGroup>(url,{params:httpParams});
  }

  createTaskGroup(body:any){
    const url = this.baseUrl + "/TaskGroup/createTaskGroup";
    return this.http.post(url,body);
  }
  updateTaskGroup(body : TaskGroup)
  {
    const url = this.baseUrl + "/TaskGroup/updateTaskGroup";
    return this.http.put(url,body);
  }
}
