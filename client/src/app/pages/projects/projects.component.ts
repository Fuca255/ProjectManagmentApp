import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CreateProjectOverlayComponent } from '../../components/create-project-overlay/create-project-overlay.component';
import { ProjectFilter } from '../../models/project/project-filter';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { Project } from '../../models/project/project';


@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css'
})
export class ProjectsComponent implements OnInit{

  @Input() filter!: ProjectFilter;
  showProjectPreview: boolean = true;
  showCreateButton: boolean = true;
  projectText: string = 'Projects';

  constructor(private dialog: MatDialog) { 

  }

  searchValueProjectChanged = new EventEmitter< { searchText: string }>();

  searchFilterProjectChanged = new EventEmitter<{filter: ProjectFilter}>();

  searchSortProjectChanged = new EventEmitter<{filter: ProjectFilter}>();

  @Output() createdObject = new EventEmitter<Project>();

  private overlay = inject(MatDialog);

  private overlay2 = inject(MatDialog);
  public isManager:boolean = false;
  public isUser : boolean = false;
  public isWorker : boolean = false;
  
  public jwt_service = inject(JwtDecoderService);

  ngOnInit(){
    let user = this.jwt_service.getLoggedUser();
    if(user!==null)
    {
      if(user.role==="Project Manager")
        this.isManager = true;
      else if(user.role==="User")
        this.isUser = true;
      else if(user.role==="Project Worker")
        this.isWorker = true;
    }
  }
  
  private CreatedProject! : Project;
  getProject()
  {
    if(this.CreatedProject!==undefined)
      this.createdObject.emit(this.CreatedProject);
  }
  openCreateOverlay(): void {
    const dialogRef = this.dialog.open(CreateProjectOverlayComponent, {
    });

    dialogRef.afterClosed().subscribe(result => {

      this.CreatedProject = result;
      this.getProject();
      this.showProjectPreview = true;
      this.showCreateButton = true;
      this.projectText = 'Project list';
    });
    
    this.showProjectPreview = false;
    this.showCreateButton = false;
    this.projectText = 'Project list/Create project';
  }

  onSearch(event: KeyboardEvent){
    const searchText = (event.target as HTMLInputElement).value;
    this.searchValueProjectChanged.emit({ searchText });
  }

}