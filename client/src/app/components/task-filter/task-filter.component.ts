import { Component, EventEmitter, Input, OnInit, inject } from '@angular/core';
import { StateService } from '../../services/state.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { PriorityService } from '../../services/priority.service';
import { ProjectService } from '../../services/project.service';
import { Priority } from '../../models/priority/priority';
import { TaskFilter } from '../../models/task/task-filter';
import { State } from '../../models/state/state';
import { Project } from '../../models/project/project';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user/user';

@Component({
  selector: 'app-task-filter',
  templateUrl: './task-filter.component.html',
  styleUrl: './task-filter.component.css'
})
export class TaskFilterComponent implements OnInit {
  
  @Input() public filter! : TaskFilter;
  private router = inject(Router);
  private projectId :number = 0;
  private state_service = inject(StateService);
  private user_service = inject(UserService);
  private spinner = inject(NgxSpinnerService);
  private route = inject(ActivatedRoute);
  filterEmiter = new EventEmitter<TaskFilter>();
  private priority_service = inject(PriorityService);
  private projectService = inject(ProjectService);
  public states : State[] = [];
  public priorities : Priority[] = [];
  public EndRange : Date[] | undefined;
  public StartRange : Date[] | undefined;
  public projects : Project[] = [];
  public selectedProjects : Project[] = [];
  public users : User[] = [];
  public selectedUsers : User[] = []
  public sortList : string[] = [
    "Last Updated",
    "State",
    "Priority",
    "Title",
    "Start Date",
    "End Date",
    "Progress"
  ];

  public notProjectDetails : boolean = this.router.url.split("/")[1]==="tasks";

  selectedPrios : Priority[] = [];
  selectedStates : State[] = [];
  public percentageValues :number[] = [0, 100];
  //uzimanje podataka
  ngOnInit(): void {
    this.spinner.show();
    if(this.notProjectDetails)
    {
      this.projectService.getUserFilterProjects().subscribe({
        next : (res:Project[])=>{this.projects = res; this.spinner.hide(); console.log(res);}
      });
    }
    else
    {
      this.projectId = Number(this.route.snapshot.paramMap.get('projectId'));
      this.user_service.getProjectUsers(this.projectId).subscribe({
        next : (users : User[]) => {
          users.forEach(user => {
            user.fulname = user.name +" " +user.lastname;
          });
          this.users = users;
        }
      });
    }
    this.state_service.fetchAllStates().subscribe({
      next : (res:State[]) => {this.states = res; this.spinner.hide();}
    });
    this.priority_service.getPriorities().subscribe({
      next : (res:Priority[])=>{this.priorities=res;}
    });
  }
  sendSortFilter(mode:number){
    this.filter.sortFlag = mode;
    this.sendFilter();
  }
  //slanje filter
  sendFilter(){
    this.filter.priorityFilter = []
    this.filter.stateFilter = [];
    this.filter.projects = [];
    this.filter.user_ids = [];
    this.filter.percentageFilterFrom = this.percentageValues[0];
    this.filter.percentageFilterTo = this.percentageValues[1];
    if(this.EndRange!==null && this.EndRange!==undefined && this.EndRange.length > 0)
    {
      this.filter.endFrom = this.EndRange[0];
      this.filter.endTo = this.EndRange[1];
    }
    else
    {
      this.filter.endFrom = undefined;
      this.filter.endTo = undefined;

    }
    if(this.StartRange!==null && this.StartRange!==undefined && this.StartRange.length > 0)
    {
      this.filter.startFrom = this.StartRange[0];
      this.filter.startTo = this.StartRange[1];
    }
    else
    {
      this.filter.startFrom = undefined;
      this.filter.startTo = undefined;
    }
    if(this.selectedPrios.length > 0)
    {
      this.filter.priorityFilter = [];
      this.selectedPrios.forEach(prio => {
        this.filter.priorityFilter?.push(prio.id);
      });
    }
    if(this.notProjectDetails)
    {
      this.selectedProjects.forEach(project => {
        this.filter.projects?.push(project.id);
      });
    }
    else{
      this.selectedUsers.forEach(user => {
        this.filter.user_ids?.push(user.id);
      });
    }
    if(this.selectedStates.length > 0)
    {
      this.filter.stateFilter = [];
      this.selectedStates.forEach(state=>{
        this.filter.stateFilter?.push(state.id);
      });
    }
    // console.log(this.filter);

    this.filterEmiter.emit(this.filter);
  }
}
