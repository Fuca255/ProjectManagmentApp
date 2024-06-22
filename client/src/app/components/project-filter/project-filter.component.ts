import { Component, EventEmitter, Input, OnInit, inject } from '@angular/core';
import { ProjectFilter } from '../../models/project/project-filter';
import { StateService } from '../../services/state.service';
import { PriorityService } from '../../services/priority.service';
import { State } from '../../models/state/state';
import { Priority } from '../../models/priority/priority';
import { ProjectFilterLimit } from '../../models/project/project-filter-limit';
import { ProjectService } from '../../services/project.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-project-filter',
  templateUrl: './project-filter.component.html',
  styleUrl: './project-filter.component.css'
})
export class ProjectFilterComponent implements OnInit {
  
  @Input() filter! : ProjectFilter;
  public limit : ProjectFilterLimit ={
    budgetMax: 0,
    budgetMin: 0,
    spentMax: 0,
    spentMin: 0
  };
  searchFilterProjectChanged = new EventEmitter<{filter : ProjectFilter}>();
  private state_service = inject(StateService);
  private spinner = inject(NgxSpinnerService);
  private priority_service = inject(PriorityService);
  private projectService = inject(ProjectService);
  public states : State[] = [];
  public priorities : Priority[] = [];
  public EndRange : Date[] | undefined;
  public StartRange : Date[] | undefined;
  public sortList : string[] = [
    "Last Updated",
    "State",
    "Priority",
    "Title",
    "Budget",
    "Spent",
    "Start Date",
    "End Date",
    "Progress"
  ];

  selectedPrios : Priority[] = [];
  selectedStates : State[] = [];
  public percentageValues :number[] = [0, 100];
  public spentValues : number[] = [];
  public budgetValues : number[] = [];
  
  ngOnInit(): void {
    this.spinner.show();
    this.projectService.getProjectFilterLimits().subscribe({
      next : (res : ProjectFilterLimit)=> {
        this.limit=res;
        this.budgetValues = [this.limit.budgetMin,this.limit.budgetMax];
        this.spentValues = [this.limit.spentMin,this.limit.spentMax];
        this.spinner.hide();
      }
    });
    this.state_service.fetchAllStates().subscribe({
      next : (res:State[]) =>{this.states=res}
    });
    this.priority_service.getPriorities().subscribe({
      next : (res:Priority[])=>{this.priorities=res}
    })
  }

  sendSortFilter(mode:number):void{
    this.filter.sortFlag = mode;
    this.sendFilter();
  }

  sendFilter():void
  {
    // console.log(this.selectedPrios);
    // console.log(this.selectedStates);
    // console.log(this.EndRange);
    this.filter.priorityFilter = []
    this.filter.stateFilter = [];
    this.filter.budgetFilterFrom = this.budgetValues[0];
    this.filter.budgetFilterTo = this.budgetValues[1];
    this.filter.spentFilterFrom = this.spentValues[0];
    this.filter.spentFilterTo = this.spentValues[1];
    this.filter.percentageFilterFrom = this.percentageValues[0];
    this.filter.percentageFilterTo = this.percentageValues[1];
    if(this.EndRange!==undefined && this.EndRange!==null && this.EndRange.length > 0)
    {
      this.filter.endFrom = this.EndRange[0];
      this.filter.endTo = this.EndRange[1];
    }
    else
    {
      this.filter.endFrom = undefined;
      this.filter.endTo = undefined;

    }
    if(this.StartRange!==undefined && this.StartRange!==null && this.StartRange.length > 0)
    {
      this.filter.startFrom = this.StartRange[0];
      this.filter.startTo = this.StartRange[1];
    }
    else
    {
      this.filter.startFrom = undefined;
      this.filter.startTo = undefined;
    }
    if(this.selectedPrios.length > 0)
    {
      this.filter.priorityFilter = [];
      this.selectedPrios.forEach(prio => {
        this.filter.priorityFilter?.push(prio.id);
      });
    }
    if(this.selectedStates.length > 0)
    {
      this.filter.stateFilter = [];
      this.selectedStates.forEach(state=>{
        this.filter.stateFilter?.push(state.id);
      });
    }
    // console.log(this.filter);

    this.searchFilterProjectChanged.emit({filter:this.filter});
  }
}
