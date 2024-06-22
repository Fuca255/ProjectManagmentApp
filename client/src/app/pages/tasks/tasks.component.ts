import { Component, EventEmitter, Injector, OnInit, Output, ViewChild, inject } from '@angular/core';
import { Task } from '../../models/task/task';
import { Subscription, filter, forkJoin, interval, map, switchMap, takeUntil } from 'rxjs';
import { AssignmentService } from '../../services/assignment.service';
import { PriorityService } from '../../services/priority.service';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { TaskHeaderComponent } from '../../components/task-header/task-header.component';
import { TaskFilter } from '../../models/task/task-filter';
import { DateConverterService } from '../../services/date-converter.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { TaskFilterComponent } from '../../components/task-filter/task-filter.component';
import { TaskPaginatedObject } from '../../models/pagination/task-paginated-object';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrl: './tasks.component.css'
})
export class TasksComponent implements OnInit{
  
  // @Output() searchValueChanged = new EventEmitter<string>();
  // @Output() searchFilterChanged = new EventEmitter<TaskFilter>();
  // @Output() searchSortChanged = new EventEmitter<TaskFilter>();

  @ViewChild('taskHeader', { static: false }) taskHeaderComponent: TaskHeaderComponent | undefined;
  @ViewChild('taskFilter') taskFilterComponent: TaskFilterComponent | undefined;
  filteredTasks: Task[] = [];


  public MaxPages: number = 0;

  paginatedObject!: TaskPaginatedObject;

  isLastPage: boolean = false;

  private task_date_service = inject(DateConverterService);

  public currentPage: number = 1;

  private router = inject(Router);

  private activatedRoute = inject(ActivatedRoute);

  public filter: TaskFilter = {
    propertyName : "Last Updated",
    sortFlag : -1,
    pageNumber: 1,
    pageSize: 6
  };

  private jwtDecoder = inject(JwtDecoderService);
  // remainingTimeSubscriptions: Subscription[] = [];

  constructor(private taskService: AssignmentService, private route: ActivatedRoute) { }

  // ngOnDestroy(): void {
  //   this.remainingTimeSubscriptions.forEach(sub => sub.unsubscribe());
  // }

  ngOnInit(): void {

    this.loadTasks();

    this.activatedRoute.paramMap.subscribe(params => {
      if (params.has('pageNumber')) {
        // console.log(params.get('pageNumber'))
        this.currentPage = parseInt(params.get('pageNumber')!);
        this.filter.pageNumber = this.currentPage;
      }
    });
  }

  ngAfterViewInit() {
    this.taskHeaderComponent?.searchValueChanged.subscribe(searchValue => {
      this.filter.searchTitle = searchValue.searchText;
      // this.filterbezpag.searchTitle = searchValue.searchText;
      this.navigateToPage(1);
      this.loadTasks();
    });
    this.taskFilterComponent?.filterEmiter.subscribe(filter=>{
      this.navigateToPage(1);
      let text = "";
      if(this.filter.searchTitle)
        text = this.filter.searchTitle
      this.filter = filter;
      this.filter.searchTitle = text;
      this.loadTasks();
    });
  }


  loadTasks(){

    let token = this.jwtDecoder.getToken();
    let id = 0;
    if(token!=null)
    {
      let decode = this.jwtDecoder.decodeToken(token);
      id = decode.user_id;

      this.taskService.getPaginatedObjectAssignmentsByUser(id, this.filter).subscribe({
        next:(pag: TaskPaginatedObject)=>{
          this.paginatedObject = pag;
          this.filteredTasks = pag.assignments;
          this.MaxPages = Math.ceil(this.paginatedObject.maxAssignments / this.filter.pageSize!);
          this.filteredTasks.forEach(task => {
            this.task_date_service.setDateParametersForTask(task);
          });
        }
      });
    }
  }


  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.filter.pageNumber = this.currentPage;
      this.router.navigate(['/tasks', this.currentPage]);
      this.loadTasks();
    }
    // console.log(this.getTotalPages());
  }
  
  nextPage() {
    if(this.currentPage < this.MaxPages){
      this.currentPage++;
      this.filter.pageNumber = this.currentPage;
      this.router.navigate(['/tasks', this.currentPage]);
      this.loadTasks();
    }
    this.updateIsLastPage();
    // console.log(this.getTotalPages());
  }

  updateIsLastPage() {
    this.isLastPage = this.currentPage === this.MaxPages;
  }


  navigateToPage(pageNumber: number) {
    this.currentPage = pageNumber;
    this.filter.pageNumber = pageNumber;
    this.router.navigate(['/tasks', pageNumber]);
    this.loadTasks();
  }

  getDisplayedPageRange(currentPage: number, totalPages: number): number[] {
    const maxDisplayedPages = 3;
    let startPage = Math.max(1, currentPage - Math.floor(maxDisplayedPages / 2));
    const endPage = Math.min(totalPages, startPage + maxDisplayedPages - 1);
    startPage = Math.max(1, Math.min(startPage, endPage - maxDisplayedPages + 1));
    return Array.from({ length: endPage - startPage + 1 }, (_, i) => startPage + i);
  }
}
