import { ChangeDetectorRef, Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { Role } from '../../models/role';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { Task } from '../../models/task/task';
import { RoleService } from '../../services/role.service';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { UserService } from '../../services/user.service';
import { TaskDetailsComponent } from '../../pages/task-details/task-details.component';

@Component({
  selector: 'app-task-list-task',
  templateUrl: './task-list-task.component.html',
  styleUrl: './task-list-task.component.css'
})
export class TaskListTaskComponent {
  private userService = inject(UserService);
  private cdr = inject(ChangeDetectorRef);
  // private jwt_service = inject(JwtDecoderService);
  // private role_service = inject(RoleService);
  @Input() task!: Task;
  @Input()userProjectRole! : Role;

  @Output() isUpdated = new EventEmitter<boolean>();
  private router = inject(Router);
  private dialog = inject(MatDialog);
  public asigneesString = "";
  public realAssignessStr = "";

  isClick = false;
  initialX: number | undefined;
  initialY: number | undefined;
  threshold = 1;
  picture!: string;

  ngOnInit(): void {
    // let loggedUser = this.jwt_service.getLoggedUser();
    // if(loggedUser!==undefined)
    //   this.role_service.getUserProjectRole(loggedUser.user_id,this.task.taskGroup.projectId).subscribe({
    //     next:(role:Role)=>{
    //       this.userProjectRole = role;
    //     }
    //   });
    if(this.task!==undefined)
    {
      this.asigneesString = this.task.assignees[0].name + " "+ this.task.assignees[0].lastname;
      this.realAssignessStr = this.task.assignees[0].name + " "+ this.task.assignees[0].lastname;
      for (let index = 1; index < this.task.assignees.length; index++) {
        const element = this.task.assignees[index];
        if(index < 2)
          this.asigneesString+=", " + element.name + " "+element.lastname;
        
        this.realAssignessStr+=", " + element.name + " "+element.lastname;
      }
      if(this.task.assignees.length >= 3)
        this.asigneesString+=" +"+(this.task.assignees.length - 2);
      // console.log(this.asigneesString);
    }
    this.userService.picture$.subscribe(picture => {
      this.picture = picture;
      if(this.picture == '')
        this.profilePicture(this.task.owner.id);
      
      this.cdr.detectChanges();
    });
  }

  getStateClass(state: string): string{
    switch(state){
        case 'Not started':
            return 'not-started';
        case 'Ready':
            return 'ready';
        case 'In Progress':
            return 'in-progress';
        case 'Blocked':
            return 'blocked';
        case 'Done':
            return 'done';
        case 'Cancelled':
            return 'cancelled';
        default:
            return '';
    }
  }
  
  extractNumber(text: string): number | null {
    const regex = /\d+/;
    const match = regex.exec(text);
    return match ? parseInt(match[0], 10) : null;
  }

  extractAfterNumber(text: string): string | null {
    const regex = /\d+/;
    const match = regex.exec(text);
    if (match) {
      const startOfTextAfterNumber = match.index + match[0].length;
      return text.substring(startOfTextAfterNumber);
    } else {
      return null;
    }
  }



  extractNumberAndText(text: string): { number: number | null; text: string | null } {
    const regex = /(\d+\.?\d*)/; // Match digits, optional decimal, and digits
    const match = regex.exec(text);
  
    if (match) {
      const extractedNumber = parseFloat(match[0]); // Convert matched string to number (including decimals)
      const startOfTextAfterNumber = match.index + match[0].length;
      const extractedText = text.substring(startOfTextAfterNumber);
      return { number: extractedNumber, text: extractedText };
    } else {
      return { number: null, text: null }; // No number found
    }
  }

  getCorrectTime(time: string,timeDifference: number): string{
    const number = this.extractNumber(time);
    // console.log(number)
    // console.log(timeDifference)
    if(number){
      if(time.includes('days')){
        const days = number * 1000 * 60 * 60 * 24;
        if(days<timeDifference/3){
          return 'low-time';
        }
        else if(days<((2*timeDifference)/3)){
          return 'medium-time';
        }
        else{
          return 'high-time';
        }
      }
      else if(time.includes('hours')){
        const hours = number * 1000 * 60 * 60;
        if(hours<timeDifference/3){
          return 'low-time';
        }
        else if(hours<((2*timeDifference)/3)){
          return 'medium-time';
        }
        else{
          return 'high-time';
        }
      }
      else if(time.includes('minutes')){
        const minutes = number * 1000 * 60;
        // console.log(minutes)
        // console.log(timeDifference)
        if(minutes>=((2*timeDifference)/3)){
          return 'high-time';
        }
        else if(minutes>=(timeDifference/3) && minutes<((2*timeDifference)/3)){
          return 'medium-time';
        }
        else{
          return 'low-time';
        }
      }
      else{
        return 'low-time';
      }
    }
    else{
      return 'low-time';
    }
    
  }

  getFloorRemaining(remaining: string): string{
    const string = this.extractNumberAndText(remaining);
    if(string && string.number && string.text){
      const floor = Math.floor(string.number)
      // console.log(floor)
      return (floor).toString() + string.text;
    }
    else{
      return 'No more time';
    }
  }

  check(event: MouseEvent){
    this.isClick = true;
    this.initialX = event.clientX;
    this.initialY = event.clientY;
  }

  openShowTaskOverlay(event: MouseEvent): void {
    let deltaX = event.clientX;
    let deltaY = event.clientY;

    if(this.initialX && this.initialY){
      deltaX = Math.abs(event.clientX - this.initialX);
      deltaY = Math.abs(event.clientY - this.initialY);
    }

    if (this.isClick && deltaX < this.threshold && deltaY < this.threshold){
      const dialogRef = this.dialog.open(TaskDetailsComponent, {
        data:[this.task, 0,this.userProjectRole]
      });

      dialogRef.afterClosed().subscribe(result => {
        this.task = result;
        this.isUpdated.emit(true);
      });
    }else {
    this.isClick = false;
    }
    
  }
  public getPriorityClass()
  {
    if(this.task.priority.description.toLowerCase() === "low")
      return "low";
    if(this.task.priority.description.toLowerCase() === "at risk")
      return "at-risk";
    if(this.task.priority.description.toLowerCase() === "medium")
      return "medium";
    if(this.task.priority.description.toLowerCase() === "high")
      return "high";
    
    return "";


  }


  profilePicture(userId: number) {
    this.userService.profilePicture(userId).subscribe({
      next: (message: { profilePicture: string, type: string }) => {
        if(message.profilePicture)
          this.picture = `data:${message.type};base64,${message.profilePicture}`;
        else 
          this.picture = "../../../assets/pictures/defaultpfp.svg";
      }, error: (err) => {
        console.error('Error retrieving profile picture:', err);
      }
    });
  }
}
