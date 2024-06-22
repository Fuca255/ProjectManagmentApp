import { Injectable } from '@angular/core';
import { TaskFilter } from '../models/task/task-filter';

@Injectable({
  providedIn: 'root'
})
export class FilterDataService {

  private filter: TaskFilter | null = null;

  setFilter(filter: TaskFilter): void {
    this.filter = filter;
  }

  getFilter(): TaskFilter | null {
    return this.filter;
  }
}