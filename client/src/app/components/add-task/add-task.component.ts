import { Component, Inject, Input, OnInit, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { State } from '../../models/state/state';
import { StateService } from '../../services/state.service';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user/user';
import { TaskGroup } from '../../models/task/task-group';
import { TaskGroupService } from '../../services/task-group.service';
import { AssignmentService } from '../../services/assignment.service';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { PriorityService } from '../../services/priority.service';
import { Priority } from '../../models/priority/priority';
import { Task } from '../../models/task/task';
import { CreateTask } from '../../models/task/create-task';
import { NgxSpinnerService } from 'ngx-spinner';
import { group } from 'console';
import { NotificationService } from '../../services/notification.service';
import { CreateNotification } from '../../models/notifications/create-notification';

@Component({
  selector: 'app-add-task',
  templateUrl: './add-task.component.html',
  styleUrl: './add-task.component.css'
})
export class AddTaskComponent implements OnInit {

  private data : any =  inject(MAT_DIALOG_DATA)
  private notification_service = inject(NotificationService);
  public state : State;
  public showDropdown: boolean;
  public showDropdownTask : boolean;
  public users : User[] = [];
  public taskGroups : TaskGroup[] = [];
  public message : string = "";
  public createTaskObj : CreateTask = {
    title: "",
    type: "neki tip",
    description: "",
    start: 0,
    end: 0,
    taskGroupId: 0,
    dependentOn: [],
    assignees: [],
    stateId: 0,
    priorityId: 4,
    owner: 0
  };

  
  // "taskGroupId": 0,
  // "userIds": [
  //   0
  // ],
  // "start": "2024-04-06T16:22:07.002Z",
  // "end": "2024-04-06T16:22:07.002Z",
  // "dependentOn": [
  //   0
  // ],
  // "title": "string",
  // "type": "string",
  // "description": "string",
  // "priorityId": 0,
  // "stateId": 0
  
  //ovako moze da se dodaje umesto da se pise u konstruktor
  private dialogRef = inject(MatDialogRef<AddTaskComponent>);
  private stateService =  inject(StateService);
  private userService = inject(UserService);
  private taskGroupService =  inject(TaskGroupService);
  private userId : number = 0;
  private assignmentService = inject(AssignmentService);
  private decoder = inject(JwtDecoderService);
  private priority_service = inject(PriorityService);
  private spinner = inject(NgxSpinnerService);
  public assignments : Task[] = [];
  public priorities : Priority[] = [];
  public selectedPriority! : Priority;
  public selectedState! : State;
  public states : State[] = [];
  public StartDate! : Date;
  public EndDate! : Date;
  public selectedTaskGroup! : TaskGroup;
  public selectedAssignees : User[] = [];
  public selectedDependentOn : Task[] = [];



  constructor() { 
    this.showDropdown = false;
    this.showDropdownTask = false;
    this.state = {
      id : 0,
      name : ""
    }
  }
  ngOnInit(): void {
    this.spinner.show();
    let token = this.decoder.getToken();
    if(token!=null)
    {
      let decode = this.decoder.decodeToken(token);
      this.userId = decode.user_id;
    }
      // this.assignments = this.data[2];
    this.assignmentService.getAllProjectAssignments(this.data[1]).subscribe({
      next: (tasks: Task[]) => {
        this.assignments = tasks;
      }
    });
  
    this.priority_service.getPriorities().subscribe({
      next : (prios :Priority[]) =>{
        this.priorities = prios;
        this.selectedPriority = prios[prios.length-1];
      }
    })
    this.stateService.fetchAllStates().subscribe({
      next: (states: State[])=>{
        this.states = states;
        let a = states.find(s=>s.id==this.data[0]);
        if(a!==undefined)
          this.selectedState = a;
      }
    });
    // console.log(this.data);
    this.userService.getProjectUsers(Number(this.data[1])).subscribe({
      next : (users: User[])=>{
        this.users = [];
        users.forEach(element => {
          element.fulname = element.name + " "+ element.lastname;
          if(element.isActivated)
            this.users.push(element);
        });
      }
    });

    this.taskGroupService.getAllProjectTaskGroups(this.data[1]).subscribe({
      next : (groups: TaskGroup[])=> {
        this.taskGroups = groups;
        if(this.data[2]===undefined) 
          this.selectedTaskGroup = groups[0];
        else
        {
          let group = groups.filter(g=>g.id==this.data[2].id);
          if(group!==undefined)
            this.selectedTaskGroup = group[0];
        } 
        this.spinner.hide();
      },
      // error:(error: any)=> console.log(error)
    });

    
  }

  createTask() : void{
    this.spinner.show();
    this.message = "";
    this.createTaskObj.assignees = [];
    this.createTaskObj.stateId = this.selectedState.id;
    this.createTaskObj.priorityId = this.selectedPriority.id;
    this.createTaskObj.taskGroupId = this.selectedTaskGroup.id;
    this.createTaskObj.type = "neki tip";
    if(this.EndDate===undefined || this.StartDate===undefined)
    {
      this.message = "Form is not filled properly, check if you entered everything correctly.";
      this.spinner.hide();
      return;
    }
    this.createTaskObj.end = new Date(this.EndDate.toDateString());
    this.createTaskObj.start = new Date(this.StartDate.toDateString());
    this.createTaskObj.dependentOn = [];
    this.createTaskObj.owner = this.userId;
    this.selectedAssignees.forEach(element => {
      this.createTaskObj.assignees.push(element.id);
    });
    this.selectedDependentOn.forEach(element => {
      this.createTaskObj.dependentOn.push(element.id);
    });

    // console.log(this.createTaskObj);
    if(this.createTaskObj.assignees.length == 0 || this.createTaskObj.title==="")
    {
      this.message = "Form is not filled properly, check if you entered everything correctly.";
      this.spinner.hide();
      return;
    }

    let todayTime = new Date();
    this.createTaskObj.end = new Date(this.createTaskObj.end);
    this.createTaskObj.start = new Date(this.createTaskObj.start);
    //provera da li je end pre starta
    if(this.createTaskObj.end < this.createTaskObj.start)
    {
      this.message = "End date comes before start date.";
      this.spinner.hide();
      return;
    }
    // todayTime.setHours(0, 0, 0, 0);
    // this.createTaskObj.start.setHours(0,0,0,0);
    // // console.log(todayTime);
    // // console.log(this.createTaskObj.start);
    // //prvera da li je start date pre danasnjeg
    // if(this.createTaskObj.start < todayTime)
    // {
    //   this.message = "Start date comes before today.";
    //   return;

    // }
    // console.log(this.createTaskObj);
    this.assignmentService.createAssignment(this.createTaskObj).subscribe({
      next : (asign : Task) => {
        // console.log("Creation succesful");
        // confirm("Task created successfully!");
        let create :CreateNotification = {
          userIds: this.createTaskObj.assignees,
          title: 'New task has been assigned to you',
          description: `You have been assigned to task '${asign.title}'`
        }
        this.notification_service.sendNotifcation(create).then(()=>{
          // console.log("message sent");
        }).catch((err)=>{console.log(err)})
        this.spinner.hide();
        this.dialogRef.close(1);
      },
      // error: (error)=> console.log(error)
    });

     
  }
  closeOverlay(): void {
    // Close the overlay dialog
    this.dialogRef.close();
  }
}
