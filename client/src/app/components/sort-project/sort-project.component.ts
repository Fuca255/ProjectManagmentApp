import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectFilter } from '../../models/project/project-filter';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SortprojectdataService } from '../../services/sortprojectdata.service';

@Component({
  selector: 'app-sort-project',
  templateUrl: './sort-project.component.html',
  styleUrl: './sort-project.component.css'
})
export class SortProjectComponent {
  private data : any =  inject(MAT_DIALOG_DATA)

  filter: ProjectFilter = {};

  sortGroup: FormGroup;

  private sortDialog = inject(MatDialogRef<SortProjectComponent>);

  constructor(private fb: FormBuilder, private sortDataService: SortprojectdataService){
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
    // console.log(this.sortGroup.get('property')?.value)
    this.filter.propertyName = this.sortGroup.get('property')?.value;
    this.filter.sortFlag = this.sortGroup.get('flag')?.value;

    this.sortDataService.setFilter(this.filter);
    this.sortDialog.close(this.filter);
  }
}
