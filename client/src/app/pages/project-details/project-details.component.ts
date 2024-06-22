import { Component, HostListener, OnInit, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddTaskComponent } from '../../components/add-task/add-task.component';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { ProjectService } from '../../services/project.service';
import { Project } from '../../models/project/project';
import { NgxSpinnerService } from 'ngx-spinner';
import {MatIconModule} from '@angular/material/icon';
import {MatDividerModule} from '@angular/material/divider';
import {MatButtonModule} from '@angular/material/button';
import { Role } from '../../models/role';
import { JwtDecoderService } from '../../services/jwt-decoder.service';


@Component({
  selector: 'app-project-details',
  templateUrl: './project-details.component.html',
  styleUrl: './project-details.component.css',
  
})
export class ProjectDetailsComponent implements OnInit{
  showProjectDetails: boolean = true;
  spinner = inject(NgxSpinnerService);
  showCreateButton: boolean = true;
  projectText: string = 'Project details';
  user_project_role! : Role;
  searchText : string = "";

  currentView: string = 'kanban';
  scrolledDown: boolean = false;
  projectName: string = '';

  constructor(private dialog: MatDialog, private projectService: ProjectService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.spinner.show();
    this.route.url.subscribe(urlSegments => {
      const projectId = +urlSegments[urlSegments.length - 1].path;
      this.fetchProjectName(projectId);
      this.spinner.hide();
    });

    const savedView = localStorage.getItem('currentView');
    if (savedView) {
      this.currentView = savedView;
    }
  }



  fetchProjectName(projectId: number): void {
    this.projectService.getProjectById(projectId)
    .subscribe((project: Project) => {
        this.projectName = project.title; 
    });
  }

  changeView(view: string): void {
    this.currentView = view;
    localStorage.setItem('currentView', view);
    
  }
  getSearchText(text: string)
  {
    this.searchText = text;
  }

  @HostListener('window:scroll')
onScroll() {
  if (window.pageYOffset > 50) {
    this.scrolledDown = true;
  } else {
    this.scrolledDown = false;
  }
}
}
