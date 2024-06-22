import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, OnInit, inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from '../environments/environment';
import { jwtDecode } from 'jwt-decode';
import { Project } from '../models/project/project';
import { JwtDecoderService } from './jwt-decoder.service';
import { ProjectFilter } from '../models/project/project-filter';
import { UpdateProject } from '../models/project/update-project';
import { ProjectFilterLimit } from '../models/project/project-filter-limit';
import { ProjectPaginatedObject } from '../models/pagination/project-paginated-object';

@Injectable({
  providedIn: 'root'
})
export class ProjectService implements OnInit{
  
  userId: number = -1;
  baseUrl = environment.apiUrl;
  private jwtDecoder = inject(JwtDecoderService);

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    let token = this.jwtDecoder.getToken();
    if(token!=null)
    {
      let decode = this.jwtDecoder.decodeToken(token);
      this.userId = decode.user_id;
    }
  }
  getProjectFilterLimits() : Observable<ProjectFilterLimit>
  {
    let loggedUser = this.jwtDecoder.getLoggedUser();
    const url = this.baseUrl + "/Project/getProjectLimits/"+loggedUser.user_id;
    return this.http.get<ProjectFilterLimit>(url);
  }
  getUserFilterProjects(): Observable<Project[]>{
    let token = this.jwtDecoder.getToken();
    let id = 0;
    if(token!=null)
    {
      let decode = this.jwtDecoder.decodeToken(token);
      id = decode.user_id;
    }
    const url = this.baseUrl + "/Project/getFilterProjectsByUser/"+id;
    return this.http.get<Project[]>(url);

  }
  getProjectsData(params: ProjectFilter = {}): Observable<Project[]> {
    let token = this.jwtDecoder.getToken();
    let id = 0;
    if(token!=null)
    {
      let decode = this.jwtDecoder.decodeToken(token);
      id = decode.user_id;
    }
    //deo za uzimanje projekata
    if (id===0) {
      return of([]);
    }

    let httpParams = this.bindProjectParams(params);

    const apiUrl = `${this.baseUrl}/Project/userProjects/${id}`;
    return this.http.get<Project[]>(apiUrl,{ params: httpParams });
  }
  getProjectsPaginatedData(params: ProjectFilter = {}): Observable<ProjectPaginatedObject> {
    let token = this.jwtDecoder.getToken();
    let id = 0;
    if(token!=null)
    {
      let decode = this.jwtDecoder.decodeToken(token);
      id = decode.user_id;
    }
    //deo za uzimanje projekata
    if (id===0) {
      return of();
    }

    let httpParams = this.bindProjectParams(params);

    const apiUrl = `${this.baseUrl}/Project/paginatedUserProjects/${id}`;
    return this.http.get<ProjectPaginatedObject>(apiUrl,{ params: httpParams });
  }

  //pravljenje projekta
  createProject(project: any): Observable<Project> {
    const url = this.baseUrl + "/Project/create";
    const body = {
      userIds : project.userIds,
      priorityId : project.priorityId,
      title : project.title,
      start : project.start,
      end : project.end,
      budget : project.budget,
      description : project.description,
      type : project.type,
      userProjectRoleIds : project.userProjectRoleIds,
      ownerId : project.ownerId
    }
    return this.http.post<Project>(url,body);
  }

  getProjectById(projectId: number): Observable<Project> {
    const url = `${this.baseUrl}/Project/getProject/${projectId}`; 
    return this.http.get<Project>(url);
  }

  updateProject(projectId: number, updateProjectData: UpdateProject): Observable<Project> {
    const url = `${this.baseUrl}/Project/update/${projectId}`;
    return this.http.put<Project>(url, updateProjectData);
  }
  bindProjectParams(params: ProjectFilter)
  {
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
    if(params.budgetFilterFrom){
      httpParams = httpParams.set('BudgetFilterFrom',params.budgetFilterFrom.toString());
    }
    if(params.budgetFilterTo){
      httpParams = httpParams.set('BudgetFilterTo',params.budgetFilterTo.toString());
    }
    if(params.spentFilterFrom){
      httpParams = httpParams.set('SpentFilterFrom',params.spentFilterFrom.toString());
    }
    if(params.spentFilterTo){
      httpParams = httpParams.set('SpentFilterTo',params.spentFilterTo.toString());
    }
    if(params.pageNumber){
      httpParams = httpParams.set('PageNumber',params.pageNumber.toString());
    }
    if(params.pageSize){
      httpParams = httpParams.set('PageSize',params.pageSize.toString());
    }
    return httpParams;
  }
}