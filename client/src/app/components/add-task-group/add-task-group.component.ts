import { Component, OnInit, inject } from '@angular/core';
import { TaskGroupService } from '../../services/task-group.service';
import { Task } from '../../models/task/task';
import { TaskGroup } from '../../models/task/task-group';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-add-task-group',
  templateUrl: './add-task-group.component.html',
  styleUrl: './add-task-group.component.css'
})
export class AddTaskGroupComponent implements OnInit{
  private task_group_service = inject(TaskGroupService);
  private spinner = inject(NgxSpinnerService);
  private data = inject(MAT_DIALOG_DATA);
  private dialogRef = inject(MatDialogRef<AddTaskGroupComponent>);
  public title: string = "";
  public createTaskGroup : TaskGroup = {
    id: 0,
    title: '',
    projectId: 0,
    parentTaskGroupId: 0,
    assignments: []
  };
  IsEdit = false;
  ngOnInit(): void {
    this.createTaskGroup.projectId = this.data[0];
    this.createTaskGroup.parentTaskGroupId = this.data[1].id;
    if(this.data[2]!==undefined)
    {
      // console.log("edit je")
      this.createTaskGroup.id = this.data[1].id;
      this.createTaskGroup.parentTaskGroupId = this.data[1].parentTaskGroupId;
      this.createTaskGroup.title = this.data[1].title;
      this.title = this.data[1].title;
      this.IsEdit = true;
      // console.log(this.createTaskGroup);
    }
  }
  editGroup(){
    if(this.title!=="")
      {
        // console.log("kliknuo edit");
        this.spinner.show();
        this.createTaskGroup.title = this.title;
        // console.log(this.createTaskGroup);
        this.task_group_service.updateTaskGroup(this.createTaskGroup).subscribe({
          next : (group:any)=>{
            this.spinner.hide();
            this.dialogRef.close(1);
          }
        });
      }
      // else
        // alert("Please enter the new name");
  }
  createGroup()
  {
    if(this.title!=="")
    {
      this.spinner.show();
      this.createTaskGroup.title = this.title;
      this.task_group_service.createTaskGroup(this.createTaskGroup).subscribe({
        next : (group:any)=>{
          this.spinner.hide();
          this.dialogRef.close(1);
        }
      });
    }
    // else
    //   alert("Please enter the name");
  }

}
