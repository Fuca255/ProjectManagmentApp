import { Component, ElementRef, EventEmitter, OnDestroy, OnInit, Output, ViewChild, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AssignmentService } from '../../services/assignment.service';
import { StateService } from '../../services/state.service';
import { Priority } from '../../models/priority/priority';
import { State } from '../../models/state/state';
import { User } from '../../models/user/user';
import { UserService } from '../../services/user.service';
import { PriorityService } from '../../services/priority.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Task } from '../../models/task/task';
import { CommentService } from '../../services/comment.service';
import { Comment } from '../../models/comment/comment';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { UpdateTask } from '../../models/task/update-task';

import { DateConverterService } from '../../services/date-converter.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Role } from '../../models/role';
import { Answer } from '../../models/comment/answer';

import { CreateNotification } from '../../models/notifications/create-notification';
import { NotificationService } from '../../services/notification.service';
import { Notify } from '../../models/notifications/notify';
import { NgToastService } from 'ng-angular-popup';
import {modules} from '..//../services/quillToolbar-data'


@Component({
  selector: 'app-task-details',
  templateUrl: './task-details.component.html',
  styleUrl: './task-details.component.css'
})
export class TaskDetailsComponent implements OnInit,OnDestroy{

  @ViewChild('overlayContainer', { static: true }) overlayContainer!: ElementRef;

  private assignment_service = inject(AssignmentService);
  private jwt_service = inject(JwtDecoderService);
  private state_service =  inject(StateService);
  private user_service = inject(UserService);
  private priority_service = inject(PriorityService);
  private comment_service = inject(CommentService);
  private dialogRef = inject(MatDialogRef<TaskDetailsComponent>);
  private date_task_service = inject(DateConverterService);
  private data : any =  inject(MAT_DIALOG_DATA);
  private notification_service = inject(NotificationService);
  private spinner = inject(NgxSpinnerService);

  userProjectRole!: Role;

  moduls = modules;

  notify : Notify;

  public comments : Comment[] = []
  public userId : number = 0;
  public commentText : string = "";
  public assignment! : Task;
  public dependentTasks : Task[] = [];
  public hasDependent : boolean = true;



//update props
  private selectedAssignees : number[] = [];
  private selectedDependentOn : number[] = []
  public showUpdate : boolean = false;
  public showDropdown :boolean = false;
  public showDropdownTask : boolean = false;

  public currentDueDate = "";
  public currentStartDate="";
  public priorities : Priority[] = [];
  public states : State[] = [];
  public projectAssignments:Task[] = []
  public projectUsers : User[] = [];  
  public percentageChange : number = 0;

  public picture!: string;
  public pictureLoading: boolean = true;

  public updateObj: UpdateTask = {
    taskGroupId: 0,
    userIds: [],
    start: new Date(),
    end: new Date(),
    dependentOn: [],
    stateId: 0,
    percentage: 0,
    title: '',
    type: '',
    description: '',
    priorityId: 0
  }

  public isManager:boolean = false;
  public isUser : boolean = false;
  public isWorker : boolean = false;
  public selectedPriority!:Priority;
  public selectedState! :State;
  
  public hasCompletedDependentTasks:boolean = false;
  constructor(private toast : NgToastService) {
    this.notify = new Notify(toast)
  }
  ngOnDestroy(): void {
    this.closeOverlay();
  }
  ngOnInit(): void {
    let token = this.jwt_service.getLoggedUser();
    this.userId = token.user_id;
    this.spinner.show();
    this.assignment = this.data[0];
    this.userProjectRole = this.data[2];

    // console.log(this.assignment);
    if(this.assignment.depndentOn.length > 0)
      this.hasDependent = false;

    // console.log(this.assignment);
    let user = this.jwt_service.getLoggedUser();
    if(this.userProjectRole!==undefined)
    {
      if(this.userProjectRole.name==="Project Manager")
        this.isManager = true;
      else if(this.userProjectRole.name==="Member")
        this.isUser = true;
      else if(this.userProjectRole.name==="Project Worker")
      {
        if(this.assignment.assignees.find(a=>a.id==this.userId)!==undefined)
          this.isWorker = true;
        else
          this.isUser = true;

      }
    }

    this.profilePicture(this.assignment.owner.id);

    this.assignment_service.getDependentAssignmentsFor(this.assignment.id).subscribe({
      next : (tasks : Task[]) => {
        this.dependentTasks = tasks;
        // console.log(tasks)
        if(tasks.find(a=>!a.isClosed)!==undefined)
          this.hasCompletedDependentTasks = false;
        else
          this.hasCompletedDependentTasks = true;
        this.spinner.hide();
        // console.log(tasks);
      }
    })
    this.comment_service.getAllComentsByTask(this.assignment.id).subscribe({
      next : (comments : Comment[]) =>{
        comments.forEach(comment => {
          comment.editedTime = new Date(comment.editedTime);
          comment.postTime = new Date(comment.postTime);
          comment.oldContent = comment.content;
          if(comment.user.profilePicture)
            comment.user.profilePicturePath = `data:${comment.user.pictureType};base64,${comment.user.profilePicture}`;
          else
            comment.user.profilePicturePath = "../../../assets/pictures/defaultpfp.svg";
          comment.replyOpened = false;
          comment.editOpened = false;
          comment.answers.forEach(ans => {
            ans.editedTime = new Date(ans.editedTime);
            ans.editOpened = false;
            ans.oldContent = ans.content;
            ans.postTime = new Date(ans.postTime);
            if(ans.user.profilePicture)
              ans.user.profilePicturePath = `data:${ans.user.pictureType};base64,${ans.user.profilePicture}`;
            else
            ans.user.profilePicturePath = "../../../assets/pictures/defaultpfp.svg";
        });
          this.comments = comments;
          // console.log(comments);
        });
      }
    });
  }

  //expand overlay
  expanded: boolean = false;
  toggleOverlay() {
    let elem = this.overlayContainer.nativeElement;
    if (!document.fullscreenElement) {
      this.expanded = true
      elem.requestFullscreen().catch((err: { message: any; name: any; }) => {
        console.error(`Error attempting to enable fullscreen mode: ${err.message} (${err.name})`);
      });
    } else {
      this.expanded = false
      document.exitFullscreen();
    }
  }


  updateTask()
  {
    this.selectedAssignees = [];
    this.selectedDependentOn = [];
    this.updateObj.percentage = this.assignment.percentage;
    this.updateObj.type = this.assignment.type;
    this.updateObj.description = this.assignment.description;
    this.selectedPriority = this.assignment.priority;
    this.selectedState = this.assignment.state;
    this.updateObj.start = this.assignment.start;
    this.updateObj.end = this.assignment.end;
    this.updateObj.title = this.assignment.title;
    this.updateObj.taskGroupId = this.assignment.taskGroup.id;
    this.showUpdate = true;
    this.percentageChange = this.assignment.percentage;
    //godina mesec dan
    this.currentDueDate = this.transformDate(this.assignment.end);
    this.currentStartDate = this.transformDate(this.assignment.start);

    //priorteti
    this.priority_service.getPriorities().subscribe({
      next : (prios : Priority[]) =>
        {
          this.priorities = prios;
        }
    });
    //project_users
    this.user_service.getProjectUsers(this.assignment.taskGroup.projectId).subscribe({
      next : (users : User[])=>
        {
          this.projectUsers = users;
          users.forEach(user => {
            if(this.assignment.assignees.find((a)=>a.id==user.id)!==undefined)
              {
                this.selectedAssignees.push(user.id);
              }
          });
        }
    });
    //projectAssignments
    this.assignment_service.getAllProjectAssignments(this.assignment.taskGroup.projectId).subscribe({
      next : (tasks : Task[]) =>
        {
          this.projectAssignments = tasks.filter((task)=>task.id!=this.assignment.id);

          this.dependentTasks.forEach(task => {
            task
            if(this.projectAssignments.find((a)=>a.id==task.id) !==undefined)
              {
                this.selectedDependentOn.push(task.id);
              }
          });
        }
    })
    //states
    this.state_service.fetchAllStates().subscribe({
      next : (states : State[]) => 
      {
          this.states = states;
      }
    })
  }

  createUpdateRequest()
  {
    this.spinner.show();
    this.updateObj.dependentOn = this.selectedDependentOn;
    this.updateObj.userIds = this.selectedAssignees;
    this.updateObj.priorityId = this.selectedPriority.id;
    this.updateObj.stateId = this.selectedState.id;
    
    let todayTime = new Date();
    this.updateObj.end = new Date(this.updateObj.end.toDateString());
    this.updateObj.start = new Date(this.updateObj.start.toDateString());
    //provera da li je end pre starta
    // console.log(this.updateObj.end);
    // console.log(this.updateObj.start);
    if(this.updateObj.end <= this.updateObj.start)
    {
      this.spinner.hide();
      // alert("End date comes before start date.");
      return;
    }

    if(this.selectedAssignees.length > 0)
    {
      this.assignment_service.updateAssignmentById(this.updateObj,this.assignment.id).subscribe
      ({
        next : (updatedTask :Task) =>
          {
            let usersToSendNotf: number[] = [];
            updatedTask.assignees.forEach(newUser => {
            let oldUser = this.assignment.assignees.find(u=>u.id==newUser.id);
            if(oldUser===undefined)
              usersToSendNotf.push(newUser.id);
          });
            if(usersToSendNotf.length > 0)
            {

              let create :CreateNotification = {
              userIds: usersToSendNotf,
              title: 'New task has been assigned to you',
              description: `You have been assigned to task '${updatedTask.title}'`
              }
              this.notification_service.sendNotifcation(create).then(()=>{
                console.log("message sent");
              }).catch((err)=>{console.log(err)})
            }
            if(updatedTask.percentage===100)
            {
              let create :CreateNotification = {
                userIds: [updatedTask.owner.id],
                title: 'Task closeure',
                description: `Task '${updatedTask.title}' is completed`
              }
              this.notification_service.sendNotifcation(create).then(()=>{
                // console.log("message sent");
              }).catch((err)=>{console.log(err)})

            }
            this.assignment = updatedTask;
            this.assignment.dummyTitle = this.assignment.title;
            if(this.assignment.title.length > 20)
            {
              let new_title = "";
              for (let i = 0; i < 20; i++) {
                const element = this.assignment.title[i];
                new_title+=element;
              }
              new_title+="...";
              this.assignment.dummyTitle = new_title;
            }
            this.date_task_service.setDateParametersForTask(this.assignment);
            // confirm("Task successfully updated!");
            this.showUpdate = false;
            this.closeOverlay();
            this.spinner.hide();
            this.notify.showSuccess("Task update","Task updated successfully!")
          },
          error :(error)=>
          {
              // alert("Task update failed!");
              // console.log(error);
          }
      });
    }
    else
    {
      // /alert("There must be at least one person that is assigned to this task");
    }
  }

  createComment()
  {
      let obj = {
        content: this.commentText,
        userId: this.userId,
        assignmentId: this.assignment.id
      };
      if(obj.content!=="")
      {
        this.comment_service.createComment(obj).subscribe({
          next: (comment : Comment)=>{
            // console.log(comment);
            // console.log(this.userId==comment.user.id);
            comment.editedTime = new Date(comment.editedTime);
            comment.postTime = new Date(comment.postTime);
            comment.oldContent = comment.content;
            comment.replyOpened = false;
            comment.editOpened = false;
            if(comment.user.profilePicture)
              comment.user.profilePicturePath = `data:${comment.user.pictureType};base64,${comment.user.profilePicture}`;
            else
              comment.user.profilePicturePath = "../../../assets/pictures/defaultpfp.svg";
            this.comments.push(comment);
            this.commentText = "";

            this.notify.showSuccess("Comment added","Comment added successfully!")
          }
        });
      }
  }

  closeOverlay()
  {
    
    this.dialogRef.close(this.assignment);
  }

  

  private transformDate(date : Date) : string
  {
    let splited =date.toLocaleDateString().split('/');
    for (let i = 0; i < splited.length; i++) {
      if(splited[i].length == 1)
        {
          splited[i] = "0"+splited[i];
        }
      
    }
    return splited[2] + "-" + splited[0] + "-" + splited[1];
  }

  toggleUserSelection(user_id: number,event: Event) {
    event.stopPropagation();
    if(this.isSelected(user_id))
      this.selectedAssignees = this.selectedAssignees.filter(id=>id!==user_id);
    else
      this.selectedAssignees.push(user_id);
    // console.log(this.selectedAssignees);
  }

  isSelected(userid: number) {
    return this.selectedAssignees.includes(userid);
  }
  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }

  toggleDropdownTask()
  {
    this.showDropdownTask = !this.showDropdownTask;
  }
  isSelectedTask(task_id:number)
  {
    return this.selectedDependentOn.includes(task_id);
  }

  toggleTaskSelection(task_id:number, event: Event)
  {
    event.stopPropagation();
    if(this.isSelectedTask(task_id))
      this.selectedDependentOn = this.selectedDependentOn.filter(id=>id !== task_id);
    else
      this.selectedDependentOn.push(task_id);
  }
  cancelUpdateRequest()
  {
    this.showUpdate=false;
    this.notify.showWarn("Task update","You canceled this task update!")
  }

  profilePicture(userId: number) {
    this.user_service.profilePicture(userId).subscribe({
      next: (message: { profilePicture: string, type: string }) => {
        if(message.profilePicture)
        {
          this.picture = `data:${message.type};base64,${message.profilePicture}`;
          this.pictureLoading = false;
        } else {
          this.picture = "../../../assets/pictures/defaultpfp.svg";
          this.pictureLoading = false;
        }
      },
      error: (err) => {
        console.error('Error retrieving profile picture:', err);
      }
    });
  }
  createAnswer(comment: Comment)
  {
    comment.replyOpened = false;
    let token = this.jwt_service.getToken();
    if(token!=null)
    {
      let decode = this.jwt_service.decodeToken(token);
      let body ={
        content: comment.answerContent,
        userId: decode.user_id,
        commentId: comment.id
      };
      if(body.content!=="")
      {
        this.comment_service.createAnswer(body).subscribe({
          next: (ans : Answer)=>{
            ans.editedTime = new Date(ans.editedTime);
            ans.postTime = new Date(ans.postTime);
            ans.oldContent = ans.content;
            ans.editOpened = false;
            if(ans.user.profilePicture)
              ans.user.profilePicturePath = `data:${ans.user.pictureType};base64,${ans.user.profilePicture}`;
            else
              ans.user.profilePicturePath = "../../../assets/pictures/defaultpfp.svg";
            comment.answers.push(ans);
            comment.answerContent = "";

            this.notify.showSuccess("Reply added","Reply added successfully!")
          }
        });
      }
    }
  }
  showEditBox(CorA:Comment | Answer)
  {
    if(CorA.editOpened)
    {
      CorA.editOpened = false;
      CorA.content = CorA.oldContent;
      
    }
    else
      CorA.editOpened = true;
  }

  //novi red kod komentara
  onEnter(event: any) {
      if (event.keyCode === 13) {
      const cursorPosition = event.target.selectionStart;
      const value = event.target.value;
      const newValue =
        value.substring(0, cursorPosition) +
        '\n' +
        value.substring(cursorPosition);
      event.target.value = newValue;
      event.target.setSelectionRange(cursorPosition + 1, cursorPosition + 1);
      event.preventDefault();
    }
  }
  replaceNewlines(content: string): string {
    return content.replace(/\n/g, '<br>');
  }

  updateComment(comment:Comment)
  {
    let updateData = {
      id : comment.id,
      content: comment.content
    };
    if(comment.content!=="")
    {
      this.comment_service.updateComment(updateData).subscribe({
        next: (com: Comment)=>{
          comment.content = com.content;
          comment.oldContent = com.content;
          comment.editedTime = new Date(com.editedTime);
          comment.postTime = new Date(com.postTime);
          comment.replyOpened = false;
          comment.editOpened = false;

          this.notify.showSuccess("Comment updated","Comment updated successfully!")
        }
      })
    }
    else{
      comment.editOpened = false;
      comment.content = comment.oldContent;
    }
  }
  updateAnswer(answer:Answer)
  {
    let updateData = {
      id : answer.id,
      content: answer.content
    };
    if(answer.content!=="")
    {
      this.comment_service.updateAnswer(updateData).subscribe({
        next: (com: Answer)=>{
          answer.content = com.content;
          answer.oldContent = com.content;
          answer.editedTime = new Date(com.editedTime);
          answer.postTime = new Date(com.postTime);
          answer.editOpened = false;

          this.notify.showSuccess("Answer added","Answer added successfully!")
        }
      })
    }
    else{
      answer.content = answer.oldContent;
      answer.editOpened = false;
    }
  }
  deleteAnswer(answer:Answer, comment:Comment)
  {
    let res = confirm("This reply will be deleted permanently. Are you sure you wish to proceed?");
    if(res)
    {
      let index = comment.answers.findIndex(a=>a.id===answer.id);
      comment.answers.splice(index,1);
      this.comment_service.deleteAnswer(answer.id).subscribe({
        next: (res)=>{
          // console.log("deleted successfully");
          this.notify.showSuccess("Delete comment reply", "Comment reply deleted successfuly!")
        },
        error: (err)=>{
          console.log(err);
        }
      });
    }
  }
  deleteComment(comment:Comment)
  {
    let res = confirm("This comment will be deleted permanently? Are you sure you wish to proceed?");
    if(res)
    {
      let index = this.comments.findIndex(c=>c.id===comment.id);
      this.comments.splice(index,1);
      this.comment_service.deleteComment(comment.id).subscribe({
        next: (res)=>{
          // console.log("deleted successfully");
          this.notify.showSuccess("Delete comment", "Comment deleted successfuly!")
        },
        error: (err)=>{
          console.log(err);
        }
      });
    }
  }


  showBox(comment:Comment)
  {
    comment.answerContent = "";
    // console.log(comment.replyOpened);
    if(comment.replyOpened)
      comment.replyOpened = false;
    else
      comment.replyOpened = true;
  }
  sendCloseRequest(state: boolean)
  {
    this.updateObj.userIds = [];
    this.updateObj.percentage = this.assignment.percentage;
    this.updateObj.type = this.assignment.type;
    this.updateObj.description = this.assignment.description;
    this.updateObj.priorityId = this.assignment.priority.id;
    this.updateObj.stateId = this.assignment.state.id;
    this.updateObj.start = this.assignment.start;
    this.updateObj.end = this.assignment.end;
    this.updateObj.end = new Date(this.updateObj.end.toDateString());
    this.updateObj.start = new Date(this.updateObj.start.toDateString());
    this.updateObj.title = this.assignment.title;
    this.updateObj.taskGroupId = this.assignment.taskGroup.id;
    this.updateObj.dependentOn = this.assignment.depndentOn;

    this.assignment.assignees.forEach(element => {
      this.updateObj.userIds.push(element.id);
    });
    this.updateObj.isClosed = state;

    this.assignment_service.updateAssignmentById(this.updateObj,this.assignment.id).subscribe
    ({
      next : (updatedTask :Task) =>
        {
          this.assignment = updatedTask;
          this.assignment.dummyTitle = this.assignment.title;
          if(this.assignment.title.length > 20)
          {
            let new_title = "";
            for (let i = 0; i < 20; i++) {
              const element = this.assignment.title[i];
              new_title+=element;
            }
            new_title+="...";
            this.assignment.dummyTitle = new_title;
          }
          this.date_task_service.setDateParametersForTask(this.assignment);
          // confirm("Task successfully updated!");
          this.showUpdate = false;
          // this.closeOverlay();
          this.spinner.hide();

          this.notify.showInfo("Task closed","You closed this task!")
        },
        error :(error)=>
        {
          this.notify.showWarn("Task update","Task can not be updated!")
            // console.log(error);
        }
    });
  }
}
