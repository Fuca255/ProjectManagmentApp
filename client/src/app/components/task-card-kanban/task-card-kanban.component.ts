import { Component, EventEmitter, Inject, Input, OnChanges, OnInit, Output, SimpleChanges, inject } from '@angular/core';
import { Task } from '../../models/task/task';
import { MatDialog } from '@angular/material/dialog';
import { TaskDetailsComponent } from '../../pages/task-details/task-details.component';
import { Role } from '../../models/role';

@Component({
  selector: 'app-task-card-kanban',
  templateUrl: './task-card-kanban.component.html',
  styleUrl: './task-card-kanban.component.css'
})
export class TaskCardKanbanComponent implements OnInit {
  @Input() public task! : Task;
  @Input()userProjectRole! : Role;
  private dialog = inject(MatDialog);
  ngOnInit(): void {
    this.task.end = new Date(this.task.end);
    this.task.start = new Date(this.task.start);
  }
  @Output() newItemEvent = new EventEmitter<{previous_state : number, task: Task}>();
  public hoverDateFormat()
  {
    return "Date Format: yyyy-mm-dd"
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
  public getUpdateEmitter(task:Task)
  {
    if(this.task.id===task.id)
    {
      this.task = task;
    }
  }

  public transformDate(date : Date) : string
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

  openShowTaskOverlay(): void {
    // console.log(this.task);
    const dialogRef = this.dialog.open(TaskDetailsComponent, {
      data:[this.task,0,this.userProjectRole]
    });

    dialogRef.afterClosed().subscribe(result => {
      this.newItemEvent.emit({previous_state : this.task.state.id,task:result});
      this.task = result;
    });
  }
}
