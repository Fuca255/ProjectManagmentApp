import { Component, ViewChild, ElementRef, AfterViewInit, inject, OnInit, Input, OnDestroy } from '@angular/core';
import Chart from 'chart.js/auto';
import { AssignmentService } from '../../services/assignment.service';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { Task } from '../../models/task/task';

@Component({
  selector: 'app-task-track',
  templateUrl: './task-track.component.html',
  styleUrl: './task-track.component.css'
})
export class TaskTrackComponent implements AfterViewInit,OnDestroy {
  ngOnDestroy(): void {
    this.chart.destroy();
  }
  
  @Input()tasks!: Task[];
  notStartedCount = 0;
  readyCount = 0;
  inProgressCount = 0;
  blockedCount = 0;
  doneCount = 0;
  cancelledCount = 0;
  @ViewChild('myChart') myChart!: ElementRef;

  ngAfterViewInit() {
    this.createChart();
  }

  chart:any;
	
  createChart() {
        this.tasks.forEach(task => {
          switch (task.state.id) {
            case 1:
              this.notStartedCount++;
              break;
            case 2:
              this.readyCount++;
              break;
            case 3:
              this.inProgressCount++;
              break;
            case 4:
              this.blockedCount++;
              break;
            case 5:
              this.doneCount++;
              break;
            case 6:
              this.cancelledCount++;
              break;
            default:
              break;
          }
        });

        const ctx = this.myChart.nativeElement.getContext('2d');
        this.chart = new Chart(ctx, {
          type: 'bar',
          data: {
            labels: ['Not started', 'Ready', 'In progress', 'Blocked', 'Done', 'Cancelled'],
            datasets: [{
              data: [this.notStartedCount,this.readyCount, this.inProgressCount, this.blockedCount, this.doneCount, this.cancelledCount],
              backgroundColor: [
                '#fd0363',
                '#cc095d',
                '#9c1057',
                '#3b1d4a',
                '#6b1650',
                '#0a2344'
              ],
              borderColor: [
                '#fd0363',
                '#cc095d',
                '#9c1057',
                '#3b1d4a',
                '#6b1650',
                '#0a2344'
              ],
              borderWidth: 2
            }]
          },
          options: {
            scales: {
              y: {
                beginAtZero: true
              }
            },
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
              title: {
                display: true,
                text: 'Tasks Status Overview',
                font: {
                  size: 18
                }
              },
              legend: {
                display: false 
              }
            }
          }
        });
  }
}
