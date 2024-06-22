import { Injectable } from '@angular/core';
import { ProjectFilter } from '../models/project/project-filter';

@Injectable({
  providedIn: 'root'
})
export class SortprojectdataService {

  private filter: ProjectFilter | null = null;

  setFilter(filter: ProjectFilter): void {
    this.filter = filter;
  }

  getFilter(): ProjectFilter | null {
    return this.filter;
  }
}