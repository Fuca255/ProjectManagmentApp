import { AfterViewChecked, AfterViewInit, Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild, inject, viewChild } from '@angular/core';
import { TaskGroup } from '../../models/task/task-group';
import { ListFlatNode } from '../../models/list-flat-node';
import {MatTreeFlatDataSource, MatTreeFlattener} from '@angular/material/tree';
import {FlatTreeControl} from '@angular/cdk/tree';
import { AssignmentService } from '../../services/assignment.service';
import { TaskGroupService } from '../../services/task-group.service';
import { ActivatedRoute } from '@angular/router';
import { DateConverterService } from '../../services/date-converter.service';
import { TaskFilter } from '../../models/task/task-filter';
import { TaskFilterComponent } from '../task-filter/task-filter.component';
import { filter } from 'rxjs';
import { AddTaskComponent } from '../add-task/add-task.component';
import { MatDialog } from '@angular/material/dialog';
import { AddTaskGroupComponent } from '../add-task-group/add-task-group.component';
import { Role } from '../../models/role';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { RoleService } from '../../services/role.service';


@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent implements OnInit,OnChanges,AfterViewInit{
  private convertDate = inject(DateConverterService);
  private jwt_service = inject(JwtDecoderService);
  private role_service = inject(RoleService);
  private dialog = inject(MatDialog);
  public filter : TaskFilter = {
    sortFlag : -1,
    propertyName : "Last Updated"

  }
  userProjectRole! : Role;
  @Input() searchFilter! : string; 
  @ViewChild("taskFilter") taskfilterComponent : TaskFilterComponent | undefined;  
  private _transformer = (node: TaskGroup, level: number) => {
    if(node.children!==undefined)
      {
        node.children.forEach(element => {
          if(this.isTask(element))
          {
            element.dummyTitle = element.title;
            if(element.title.length > 20)
            {
              let new_title = "";
              for (let i = 0; i < 20; i++) {
                const elem = element.title[i];
                new_title+=elem;
              }
              new_title+="...";
              element.dummyTitle = new_title;
            }
            this.convertDate.setDateParametersForTask(element);
          }
        });
      }
    return {
      expandable: !!node.children && node.children.length > 0,
      node: node,
      level: level,
    };
  };
  treeControl = new FlatTreeControl<ListFlatNode>(
    node => node.level,
    node => node.expandable,
  );

  treeFlattener = new MatTreeFlattener(
    this._transformer,
    node => node.level,
    node => node.expandable,
    node => node.children,
  );
  isTask(smth : any) : boolean{
    if(smth.assignees!==undefined)
      return true;
    return false;
  }
  isManager : boolean = false;
  isUser : boolean = false;
  isWorker : boolean = false;

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  // constructor() {
  //   this.dataSource.data = TREE_DATA;
  // }
  hasChild = (_: number, node: ListFlatNode) => {return node.expandable};
  
  private projectId : number = 0;
  private route = inject(ActivatedRoute);
  private task_service = inject(TaskGroupService);
  constructor() {
    this.projectId = Number(this.route.snapshot.paramMap.get('projectId'));
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.filter.searchTitle = this.searchFilter;
    this.loadTasks();
  }
  ngAfterViewInit(): void {
    this.taskfilterComponent?.filterEmiter.subscribe(filter=>{
      this.filter = filter;
      this.filter.searchTitle = this.searchFilter;
      // console.log(filter)
      // console.log("stigao filter");
      this.loadTasks();
    });
  }
  getIsUpdatedTask(upd: boolean)
  {
    if(upd)
      this.loadTasks();
  }

  //za ogranicavanje napravi api na beku za uzimanje project role korisnika i sa tim poredi
  ngOnInit(): void {
    let user = this.jwt_service.getLoggedUser();
    if(user!==undefined)
    {

      this.role_service.getUserProjectRole(user.user_id,this.projectId).subscribe({
        next : (role: Role)=>{
          this.userProjectRole = role;
          if(this.userProjectRole.name==="Project Manager")
            this.isManager = true;
          else if(this.userProjectRole.name==="User")
            this.isUser = true;
          else if(this.userProjectRole.name==="Project Worker")
            this.isWorker = true;
        }
      });
    }

    this.filter.searchTitle = this.searchFilter;
    this.loadTasks();
  }

  loadTasks(): void{
    this.task_service.getAllProjectTaskGroupsWithAssignments(this.projectId,this.filter).subscribe({
      next : (group: TaskGroup) => {
        this.dataSource.data = [group];
        console.log(this.dataSource.data);
        // this.treeControl.expand(this.treeControl.dataNodes[0]);
        this.treeControl.expandAll();
        // console.log(this.dataSource.data);
      }
    });
  }
  openCreateGroupOverlay(node:any)
  {
    const dialogRef = this.dialog.open(AddTaskGroupComponent, {
      data:[this.projectId,node]
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result===1)
        this.loadTasks();
    });
  }

  openEditGroupOverlay(node:any){
    const dialogRef = this.dialog.open(AddTaskGroupComponent, {
      data:[this.projectId,node,1]
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result===1)
        this.loadTasks();
    });
  }


  openCreateOverlay(node:any): void {
    const dialogRef = this.dialog.open(AddTaskComponent, {
      data:[1,this.projectId,node]
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result===1)
        this.loadTasks();
    });
    
  }
}
