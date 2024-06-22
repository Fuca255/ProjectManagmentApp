import { AfterViewInit, Component, Input, OnDestroy, OnInit, inject } from '@angular/core';
import Chart from 'chart.js/auto';
import { ProjectService } from '../../services/project.service';
import { Project } from '../../models/project/project';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { AssignmentService } from '../../services/assignment.service';
import { Task } from '../../models/task/task';

@Component({
  selector: 'app-project-weekly-analize',
  templateUrl: './project-weekly-analize.component.html',
  styleUrl: './project-weekly-analize.component.css'
})
export class ProjectWeeklyAnalizeComponent implements OnInit, OnDestroy{

  @Input() projects!: Project[];
  @Input() tasks!: Task[];
  atRiskCount = 0;
  highCount = 0;
  mediumCount = 0;
  lowCount = 0;

  patRiskCount = 0;
  phighCount = 0;
  pmediumCount = 0;
  plowCount = 0;

  constructor() { }
  ngOnDestroy(): void {
    this.chart.destroy();
  }

  ngOnInit(): void {
    this.createChart();
  }

  public chart: any;

  createChart() {
    this.tasks.forEach(task => {
      switch (task.priority.id) {
        case 1:
          this.atRiskCount++;
          break;
        case 2:
          this.highCount++;
          break;
        case 3:
          this.mediumCount++;
          break;
        case 4:
          this.lowCount++;
          break;
        default:
          break;
      }
    });

    this.projects.forEach(project => {
      switch (project.priority.id) {
        case 1:
          this.patRiskCount++;
          break;
        case 2:
          this.phighCount++;
          break;
        case 3:
          this.pmediumCount++;
          break;
        case 4:
          this.plowCount++;
          break;
        default:
          break;
      }
    });

          const daysOfWeek = ['At risk', 'High', 'Medium', 'Low'];
    
        this.chart = new Chart("MyChart", {
          type: 'bar',
          data: {
            labels: daysOfWeek,
            datasets: [
              { label: "Projects", data: [this.patRiskCount,this.phighCount,this.pmediumCount,this.plowCount], backgroundColor: '#ECEE81' },
              { label: "Tasks", data: [this.atRiskCount,this.highCount,this.mediumCount,this.lowCount], backgroundColor: '#8DDFCB' }
            ]
          },
          options: {
            aspectRatio: 2.5,
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
              legend: {
                onClick: function() {} 
              },
              title: {
                display: true,
                text: 'Priority Overview',
                font: {
                  size: 18
                }
              }
            },
            indexAxis: 'y',
            scales: {
              x: {
                title: {
                  display: true,
                  text: 'Quantity'
                },
                beginAtZero: true,
                ticks: {
                  stepSize: 1
                }
              },
              y: {
                title: {
                  display: true,
                  text: 'Priority name'
                },
                beginAtZero: true,
              }
            }
          }
        });

    
  }
}
