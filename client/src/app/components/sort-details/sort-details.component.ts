import { Component, inject } from '@angular/core';
import { State } from '../../models/state/state';
import { Priority } from '../../models/priority/priority';
import { StateService } from '../../services/state.service';
import { PriorityService } from '../../services/priority.service';
import { MAT_DIALOG_DATA,MatDialogRef } from '@angular/material/dialog';
import moment from 'moment';
import { Form, FormBuilder, FormGroup } from '@angular/forms';
import { TaskFilter } from '../../models/task/task-filter';
import { FilterDataService } from '../../services/filterdata.service';
import { ProjectService } from '../../services/project.service';
import { Project } from '../../models/project/project';
import { SortDataService } from '../../services/sortdata.service';

@Component({
  selector: 'app-sort-details',
  templateUrl: './sort-details.component.html',
  styleUrl: './sort-details.component.css'
})
export class SortDetailsComponent {

  private data : any =  inject(MAT_DIALOG_DATA)

  filter: TaskFilter = {};

  sortGroup: FormGroup;

  private sortDialog = inject(MatDialogRef<SortDetailsComponent>);

  constructor(private fb: FormBuilder, private sortDataService: SortDataService){
    this.sortGroup = this.fb.group({
      property: [''],
      flag: [null]
    })
  }

  ngOnInit(){
    this.filter = this.data[0];

    const storedFilter = this.sortDataService.getFilter();
    if(storedFilter){

      this.sortGroup.patchValue({
        property: storedFilter.propertyName,
        flag: storedFilter.sortFlag
      });

    }

  }

  resetSort(){
    this.sortGroup.get('property')?.setValue('');
    this.sortGroup.get('flag')?.setValue(null);
  }

  cancelSort(){
    this.sortDialog.close();
  }

  confirmSort(){
    this.filter.propertyName = this.sortGroup.get('property')?.value;
    this.filter.sortFlag = this.sortGroup.get('flag')?.value;

    this.sortDataService.setFilter(this.filter);
    this.sortDialog.close(this.filter);
  }

}
