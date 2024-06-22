import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output, ViewChild, inject } from '@angular/core';
import { ProjectService } from '../../services/project.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AssignmentService } from '../../services/assignment.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { EditProjectOverlayComponent } from '../edit-project-overlay/edit-project-overlay.component';
import { Project } from '../../models/project/project';
import { ProjectFilter } from '../../models/project/project-filter';
import { ProjectsComponent } from '../../pages/projects/projects.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { User } from '../../models/user/user';
import { UserService } from '../../services/user.service';
import { ProjectFilterComponent } from '../project-filter/project-filter.component';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { ProjectPaginatedObject } from '../../models/pagination/project-paginated-object';

@Component({
  selector: 'app-project-preview',
  templateUrl: './project-preview.component.html',
  styleUrls: ['./project-preview.component.css']
})

export class ProjectPreviewComponent implements OnInit {

  // @Output() searchFilterChanged = new EventEmitter<ProjectFilter>();
  // @Output() searchSortChanged = new EventEmitter<ProjectFilter>();

  @ViewChild('projectHeader', { static: false }) projectHeaderComponent: ProjectsComponent | undefined;
  @ViewChild("filter_sort_project") projectFilters : ProjectFilterComponent | undefined;

  public filter: ProjectFilter = {
    propertyName : "Last Updated",
    sortFlag : -1,
    pageNumber: 1,
  };
  public loggedUser!:any;
  public maxPages: number = 0;

  public currentPage: number = 1;

  isLastPage: boolean = false;

  private activatedRoute = inject(ActivatedRoute);

  private jwtDecoder = inject(JwtDecoderService);

  isClick = false;
  initialX: number | undefined;
  initialY: number | undefined;
  threshold = 1;
  
  isLoading: boolean = false;
  errorMessage: string = '';
  private spinner = inject(NgxSpinnerService);

  paginatedProjects!: ProjectPaginatedObject;

  data: any;
  projectsData: Project[] = [];
  users: User[] = [];
  profilePicturesLoaded = false;

  titleCharacterLimit: number = 0;
  descriptionCharacterLimit: number = 0;

  cards: any[] = [];

  showProjectPreview: boolean = true;

  assignmentCounts: { [projectId: number]: number } = {};

  constructor(private http: HttpClient, private projectService: ProjectService, private router: Router, private assignmentService: AssignmentService, private dialog: MatDialog, private userService: UserService) {
    this.calculateCharacterLimit();
  }
  public isManager:boolean = false;
  public isUser : boolean = false;
  public isWorker : boolean = false;
  
  public jwt_service = inject(JwtDecoderService);
  getCreatedProject(createdProject: Project)
  {
    this.navigateToPage(1);
  }
  ngOnInit(): void {
    this.adjustCardsPerPage();
    let user = this.jwt_service.getLoggedUser();
    if(user!==null)
    {
      this.loggedUser = user;
      if(user.role==="Project Manager")
        this.isManager = true;
      else if(user.role==="User")
        this.isUser = true;
      else if(user.role==="Project Worker")
        this.isWorker = true;
    }
    window.addEventListener('resize', this.onWindowResize.bind(this));
    this.loadProjects();
  }

  ngAfterViewInit() {
    this.projectHeaderComponent?.searchValueProjectChanged.subscribe(searchValue => {
      this.filter.searchTitle=searchValue.searchText;
      this.navigateToPage(1);
    });
    this.projectFilters?.searchFilterProjectChanged.subscribe(response=>{
      let text = "";
      if(this.filter.searchTitle)
        text = this.filter.searchTitle;
      this.filter = response.filter;
      this.filter.searchTitle = text;
  
      // console.log(this.filter);
      this.navigateToPage(1);
    });
    // this.projectHeaderComponent?.searchSortProjectChanged.subscribe(filter => {
    //   this.sortProjects(filter.filter);
    // });
  }
  onWindowResize() {
    this.adjustCardsPerPage();
  }
  loadProjects() {
    this.isLoading = true;
    this.spinner.show();
    this.projectService.getProjectsPaginatedData(this.filter).subscribe(
      (pag: ProjectPaginatedObject) => {
        this.paginatedProjects = pag;
        this.projectsData = pag.projects;
        this.maxPages =  Math.ceil(this.paginatedProjects.maxProjects / this.filter.pageSize!);
        this.projectsData.forEach(project => {
          this.users = project.users;
          this.users.forEach(user => {
            this.userService.profilePicture(user.id).subscribe({
              next: (message: { profilePicture: string, type: string }) => {
                if(message.profilePicture)
                  user.profilePicture = `data:${message.type};base64,${message.profilePicture}`;
                else
                  user.profilePicture = "../../../assets/pictures/defaultpfp.svg";
              }, 
              error: (err) => {
                console.error('Error retrieving profile picture:', err);
              },
              complete: () => {
                this.profilePicturesLoaded = true;
              }
            });
          });
        });
        this.isLoading = false;
        this.spinner.hide();
        this.projectsData.forEach(project => {
          project.truncatedTitle = this.truncate(project.title, this.titleCharacterLimit);
          project.truncatedDescription = this.truncate(project.description, this.descriptionCharacterLimit);
        });
        if (this.projectsData.length === 0) {
          this.spinner.hide();
          this.errorMessage = 'You don\'t have any projects yet.';
        }
      },
      (error) => {
        this.isLoading = false;
        this.spinner.hide();
        this.errorMessage = 'An error occurred while fetching projects.';
      }
    );
  }


  check(event: MouseEvent){
    this.isClick = true;
    this.initialX = event.clientX;
    this.initialY = event.clientY;
  }

  navigateToProjectDetails(event: MouseEvent,projectId: number) {

    let deltaX = event.clientX;
    let deltaY = event.clientY;

    if(this.initialX && this.initialY){
      deltaX = Math.abs(event.clientX - this.initialX);
      deltaY = Math.abs(event.clientY - this.initialY);
    }
    if (this.isClick && deltaX < this.threshold && deltaY < this.threshold){
      this.router.navigate(['projects','project-details',projectId]);
    }
    else{
      this.isClick = false;
    }
  }

  // Calculate character limits based on screen width
  calculateCharacterLimit() {
    const screenWidth = window.innerWidth;
    if (screenWidth < 768) {
      this.titleCharacterLimit = 12;
      this.descriptionCharacterLimit = 40;
    } else if (screenWidth >= 768 && screenWidth < 1025) {
      this.titleCharacterLimit = 15;
      this.descriptionCharacterLimit = 120;
    } else {
      this.titleCharacterLimit = 19;
      this.descriptionCharacterLimit = 210;
    }
  }

  // Truncate text to specified character limit
  truncateText() {
    this.projectsData.forEach((projectsData: { truncatedTitle: string; title: string; truncatedDescription: string; description: string; }) => {
      projectsData.truncatedTitle = this.truncate(projectsData.title, this.titleCharacterLimit);
      projectsData.truncatedDescription = this.truncate(projectsData.description, this.descriptionCharacterLimit);
    });
  }

  // Helper function to truncate text
  truncate(text: string, limit: number): string {
    if (text.length > limit) {
      return text.substring(0, limit) + '...';
    } else {
      return text;
    }
  }



  getPaginatedProjects(): any[] {
    if (!this.projectsData) {
      return [];
    }
    return this.projectsData;
  }

  previousPage(){
    if (this.currentPage > 1) {
      this.currentPage--;
      this.filter.pageNumber=this.currentPage;
      // this.loadProjects();
      this.router.navigate(['/projects', this.currentPage]);
      this.loadProjects();
    }
  }

  nextPage(){
    // console.log(this.projectsData.length)
    if(this.currentPage<this.maxPages){
      this.currentPage++;
      this.filter.pageNumber=this.currentPage;
      this.router.navigate(['/projects', this.currentPage]);
      this.loadProjects();
    }
    this.updateIsLastPage();
  }


  updateIsLastPage() {
    this.isLastPage = this.currentPage === this.maxPages;
  }


  navigateToPage(pageNumber: number) {
    this.currentPage = pageNumber;
    this.filter.pageNumber = pageNumber;
    this.router.navigate(['/projects', pageNumber]);
    this.loadProjects();
  }

  getDisplayedPageRange(currentPage: number, totalPages: number): number[] {
    const maxDisplayedPages = 3;
    let startPage = Math.max(1, currentPage - Math.floor(maxDisplayedPages / 2));
    const endPage = Math.min(totalPages, startPage + maxDisplayedPages - 1);
    startPage = Math.max(1, Math.min(startPage, endPage - maxDisplayedPages + 1));
    return Array.from({ length: endPage - startPage + 1 }, (_, i) => startPage + i);
  }


  getTeamMemberImages(project: any): string[] {
    if (!project || !project.users) {
      return [];
    }

    const teamMemberImages: string[] = [];
    for (const user of project.users) {
      if (user.profile_picture) {
        teamMemberImages.push(user.profile_picture);
      } else {
        teamMemberImages.push("../../../assets/pictures/defaultpfp.svg");
      }
    }
    
    return teamMemberImages;
  }

  adjustCardsPerPage() {
    const screenWidth = window.innerWidth;
    if (screenWidth <= 1920 && screenWidth > 1630) {
      this.filter.pageSize! = 3;
      this.loadProjects();
    } else if(screenWidth <= 1630 && screenWidth > 1371){
      this.filter.pageSize! = 2;
      this.loadProjects();
    }
    else if(screenWidth <= 1371){
      this.filter.pageSize! = 1;
      this.loadProjects();
    }
    else
      this.filter.pageSize! = 4;
      this.loadProjects();
  }

  openEditOverlay(project: Project, event: Event): void {
    event.stopPropagation();
    const dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      project: project
    };

    const dialogRef = this.dialog.open(EditProjectOverlayComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      // When overlay is closed
      // console.log("pozvan edit projekta");
      if(result!==undefined)
      {
        
        result.truncatedTitle = this.truncate(result.title, this.titleCharacterLimit);
        result.truncatedDescription = this.truncate(result.description, this.descriptionCharacterLimit);
        let ind = this.projectsData.findIndex(a=>a.id==result.id);
        this.projectsData.splice(ind,1,result);
      }
      this.showProjectPreview = true;
    });
    
    // this.showProjectPreview = false;
  }
}
