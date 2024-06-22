import { Injectable } from '@angular/core';
import { Task } from '../models/task/task';

@Injectable({
  providedIn: 'root'
})
export class DateConverterService {

  constructor() { }

  setDateParametersForTask(task : Task)
  {
    task.end = new Date(task.end);
    task.start = new Date(task.start);
    task.endMilliseconds = task.end.getMilliseconds();
    task.startMilliSeconds= task.start.getMilliseconds();
    task.endSeconds= task.end.getSeconds();
    task.endMinutes= task.end.getMinutes();
    task.endHours= task.end.getHours();
    task.endYear= task.end.getFullYear();
    task.endMonth= task.end.getMonth() + 1;
    task.endDate= task.end.getDate();
    task.startSeconds= task.start.getSeconds();
    task.startMinutes= task.start.getMinutes();
    task.startHours= task.start.getHours();
    task.startYear= task.start.getFullYear();
    task.startMonth= task.start.getMonth() + 1;
    task.startDate= task.start.getDate();
    const end = task.end;
    const start = task.start;
    const timeDifference = end.getTime()-start.getTime();
    task.timeDifference = timeDifference;
    const current = new Date();
    if(end.getTime()<current.getTime()){
      task.remaining = 'No more time';
    }
    else{
      const days = (end.getTime()-current.getTime()) / (1000 * 60 * 60 * 24);
      const hours = (end.getTime()-current.getTime()) / (1000 * 60 * 60);
      const minutes = (end.getTime()-current.getTime()) / (1000 * 60);
      if(hours>24){
        task.remaining = days.toString() + ' days';
      }
      else{
        if(minutes>60){
          task.remaining = hours.toString() + ' hours';
        }
        else{
          task.remaining = minutes.toString() + ' minutes';
        }
      }
    }
  }
}
