import { Component, ElementRef, EventEmitter, Input, OnChanges, OnInit, SimpleChanges, VERSION, ViewChild, inject } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem, CdkDropList, CdkDragMove } from '@angular/cdk/drag-drop';
import { Board } from '../../models/kanban/board.model';
import { Column } from '../../models/kanban/column.model';
import { TaskKanban } from '../../models/kanban/kanbantask';
import { TaskCardKanbanComponent } from '../task-card-kanban/task-card-kanban.component';
import { StateService } from '../../services/state.service';
import { State } from '../../models/state/state';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AddTaskComponent } from '../add-task/add-task.component';
import { AssignmentService } from '../../services/assignment.service';
import { stat } from 'fs';
import { Task } from '../../models/task/task';
import { UpdateTask } from '../../models/task/update-task';
import { NgxSpinnerService } from 'ngx-spinner';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { Role } from '../../models/role';
import { RoleService } from '../../services/role.service';
import { TaskFilter } from '../../models/task/task-filter';
import { filter } from 'rxjs';

@Component({
  selector: 'app-kanban',
  templateUrl: './kanban.component.html',
  styleUrl: './kanban.component.css'
})
export class KanbanComponent implements OnInit,OnChanges{
  name = 'Angular Material ' + VERSION.major + ' Kanban board';
   public board: Board;
   private projectId : number = 0;
   @Input() searchText!: string;
   userProjectRole! : Role;
   private state_service = inject(StateService);
   private assignment_service = inject (AssignmentService);
   private route = inject(ActivatedRoute);
   private jwt_service = inject(JwtDecoderService);
   private role_service = inject(RoleService);
   private assignments : Task[] = [];
   private spinner = inject(NgxSpinnerService);
   private states : State[] = [];
   private taskFilter : TaskFilter = {

   };
  constructor(private dialog: MatDialog)
  {
    this.projectId = Number(this.route.snapshot.paramMap.get('projectId'));
    this.board = new Board('Kanban', []);
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.taskFilter.searchTitle = this.searchText;
    this.getBoard();
    // console.log("promena");
  }

  showProjectDetails: boolean = true;
  showCreateButton: boolean = true;
  projectText: string = 'Project details';
  public isManager:boolean = false;
  public isUser : boolean = false;
  public isWorker : boolean = false;

  //otvaranje create Taska
  openCreateOverlay(column_id : string): void {
    const dialogRef = this.dialog.open(AddTaskComponent, {
      data:[column_id,this.projectId]
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getBoard();
      this.showProjectDetails = true;
      this.showCreateButton = true;
      this.projectText = 'Project details';
    });
    
    this.showProjectDetails = true;
    this.showCreateButton = true;
    this.projectText = 'Project details/Create task';
  }

  
  //pravljenje kanbana
  public ngOnInit(): void {
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
    this.getBoard();
  }

  private getBoard():void
  {
    this.spinner.show();
    this.state_service.fetchAllStates().subscribe({
      next : (states : State[])=>
    {
      this.states = states;
      let ids :string[]= []
      states.forEach(state => {
        ids.push(state.id + "");
      });
      
      //treba da se stavi od kliknutog projekta id
        this.assignment_service.getAllProjectAssignments(this.projectId,this.taskFilter).subscribe({
          next : (assignments : Task[])=>
        {
          this.assignments = assignments;
          let columns : Column[] = [];
          states.forEach(state => {
            let stateProjects : Task[] = [];
            assignments.forEach(assignment => {
              assignment.dummyTitle = assignment.title;
              if(assignment.title.length > 20)
              {
                let new_title = "";
                for (let i = 0; i < 20; i++) {
                  const element = assignment.title[i];
                  new_title+=element;
                }
                new_title+="...";
                assignment.dummyTitle = new_title;
              }
              if(assignment.state.id==state.id)
                stateProjects.push(assignment);
            });
            // console.log(stateProjects);
          columns.push(new Column(state.name,state.id + "",stateProjects,ids));
        });
        this.board.columns = columns;
        this.spinner.hide();
      },
      // error : (error:any)=> {console.log(error);}
      });
    },
    // error :(error)=> console.log(error)
  });
  }
  //menjanje iz detalaja
  public changeState(updated:any)
  {
    //brisanje iz postojece kolone
    let prev_column = updated.previous_state-1;
    let task : Task = updated.task;
    let position = this.board.columns[prev_column].tasks.findIndex(t=>t.id===task.id);
    if(prev_column+1!=task.state.id)
    {
      this.board.columns[prev_column].tasks.splice(position,1);
      //dodvanje u listu
      let next_column_id = task.state.id-1;
      this.board.columns[next_column_id].tasks = [task].concat(this.board.columns[next_column_id].tasks);
    }
    else
      this.board.columns[prev_column].tasks[position] = task;
    
  }

  //promenjeno na interfejs Assignment(bilo je TaskCardKanbanComponent)
  public dropGrid(event: CdkDragDrop<Task[]>): void {
    if(this.isManager)
      moveItemInArray(this.board.columns, event.previousIndex, event.currentIndex);
  }
  //promenjeno na interfejs Assignment(bilo je TaskCardKanbanComponent)
  public drop(event: CdkDragDrop<Task[]>): void {
    if(this.isManager)
    {
      
      if (event.previousContainer === event.container) {
        moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
      } else 
      {
      let user_ids :number[] = [];

      for (let i = 0; i < event.item.data.assignees.length; i++) {
        const element = event.item.data.assignees[i].id;
        user_ids.push(element);
        
      }
      
      let body : UpdateTask ={
        taskGroupId: event.item.data.taskGroup.id,
        userIds: user_ids,
        start: event.item.data.start,
        end: event.item.data.end,
        dependentOn: event.item.data.dependentOn,
        stateId: Number(event.container.id),
        percentage: event.item.data.percentage,
        title: event.item.data.title,
        type: event.item.data.type,
        description: event.item.data.description,
        priorityId: event.item.data.priority.id
      }
      // console.log(body);
      let state = this.states.filter(s=>s.id===Number(event.container.id));
      event.item.data.state = state[0];
      this.assignment_service.updateAssignmentById(body,event.item.data.id).subscribe({
        next : (assignment : Task)=> {
          event.item.data = assignment;
          // console.log(this.board.columns);
          // console.log(event.container);
          // console.log(event.previousContainer);
          // this.getBoard();
        }
      });
      
      transferArrayItem(event.previousContainer.data,
          event.container.data,
          event.previousIndex,
          event.currentIndex);
      }
    }
  }

  //premestanje iz kartice u karticu i scroll strane
  @ViewChild('boardWrapper')
  boardWrapper!: ElementRef;
  onDragMoved(event: CdkDragMove) {
    const scrollSensitivity = 50; 
    const scrollSpeed = 3600; 
    const boardWrapperElement = this.boardWrapper.nativeElement;
    const { x } = event.pointerPosition;
    const wrapperRect = boardWrapperElement.getBoundingClientRect();

    //console.log('Drag moved', event.pointerPosition); 
    //console.log('Wrapper rect:', wrapperRect); 

    if (x < wrapperRect.left + scrollSensitivity) {
      //console.log('Scrolling left'); 
      boardWrapperElement.scrollLeft -= scrollSpeed;
    } else if (x > wrapperRect.right - scrollSensitivity) {
      //console.log('Scrolling right'); 
      boardWrapperElement.scrollLeft += scrollSpeed;
    }
  }
}
