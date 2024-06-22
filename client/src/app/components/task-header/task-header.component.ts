import { Component, EventEmitter, Input, OnInit, ViewChild, inject } from '@angular/core';
import { ProjectService } from '../../services/project.service';
import { Project } from '../../models/project/project';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TaskFilter } from '../../models/task/task-filter';
import { SortDetailsComponent } from '../sort-details/sort-details.component';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-task-header',
  templateUrl: './task-header.component.html',
  styleUrl: './task-header.component.css'
})

export class TaskHeaderComponent implements OnInit {

  @Input() filter!: TaskFilter;

  // @ViewChild('Select') selectElement: HTMLSelectElement | undefined;

  private overlay = inject(MatDialog);

  private overlay2 = inject(MatDialog);

  projects : Project[] = [];

  selectedProjectId: number = 0;
  private spinner = inject(NgxSpinnerService);
  constructor(private projectService: ProjectService) { }

  searchValueChanged = new EventEmitter< { searchText: string }>();

  searchFilterChanged = new EventEmitter<{filter: TaskFilter}>();

  searchSortChanged = new EventEmitter<{filter: TaskFilter}>();

  ngOnInit(): void {
    this.spinner.show();
    this.fetchProjects();
  }

  fetchProjects(): void {
    this.projectService.getProjectsData().subscribe(projects => {
      this.projects = projects;
      this.spinner.hide();
    });
  }

  onSearch(event: KeyboardEvent) {

    const searchText = (event.target as HTMLInputElement).value;
    this.searchValueChanged.emit({ searchText });

  }


  openSortDialog(){
    const dialogRef = this.overlay2.open(SortDetailsComponent,{
      data:[this.filter]
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        if(result.propertyName){
          this.filter.propertyName=result.propertyName;
        }
        if(result.sortFlag){
          this.filter.sortFlag=result.sortFlag;
        }
        this.searchSortChanged.emit({filter: this.filter})
      }
    });
  }

}
