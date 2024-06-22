import { AfterViewInit, Component, OnInit, inject} from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ProjectService } from '../../services/project.service';
import { Project } from '../../models/project/project';
import { AssignmentService } from '../../services/assignment.service';
import { Task } from '../../models/task/task';
import { JwtDecoderService } from '../../services/jwt-decoder.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements AfterViewInit,OnInit {
  constructor(private spinner: NgxSpinnerService) { }
  ngOnInit(): void {
    this.project_service.getProjectsData().subscribe({
      next:(projects:Project[])=>{
        this.projects = projects;
      }
    })
    let token = this.jwtService.getToken();
    if(token!=null)
    {
      let decode = this.jwtService.decodeToken(token);
      let userid = decode.user_id;

      this.assignmentService.getAllUserAssignments(userid).subscribe(tasks => {
        this.tasks = tasks;
      });
    }
  }
  private jwtService = inject(JwtDecoderService);
  private project_service = inject(ProjectService);
  private assignmentService = inject(AssignmentService);
  public tasks!:Task[];
  public projects!:Project[];
  ngAfterViewInit(): void {
    this.spinner.show();
    this.hideSpinnerAfterLoad();
  }

  hideSpinnerAfterLoad(): void {
    setTimeout(() => {
      this.spinner.hide();
    }, 2000); 
  }
}
