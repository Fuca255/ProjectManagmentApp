import { Component, ViewChild, ElementRef, AfterViewInit, inject, Input, OnDestroy} from '@angular/core';
import Chart from 'chart.js/auto';
import { ProjectService } from '../../services/project.service';
import { Project } from '../../models/project/project';

@Component({
  selector: 'app-task-tags',
  templateUrl: './task-tags.component.html',
  styleUrl: './task-tags.component.css'
})
export class TaskTagsComponent implements AfterViewInit, OnDestroy{
  ngOnDestroy(): void {
    this.chart.destroy();
  }
  @ViewChild('myChart') myChart!: ElementRef;

  @Input() projects!: Project[];

  chart:any;
  ngAfterViewInit() {
    this.createChart();
  }

  createChart() {
    const projectNames: string[] = [];
    const completionPercentages: number[] = [];
    this.projects.forEach(project => {
      projectNames.push(project.title);
      completionPercentages.push(project.percentage);
    });

    const incompleteProjects = [];
    for (let i = 0; i < projectNames.length; i++) {
      if (completionPercentages[i] < 100) {
        incompleteProjects.push({ name: projectNames[i], percentage: completionPercentages[i] });
      }
    }

    incompleteProjects.sort((a, b) => b.percentage - a.percentage);
    const topIncompleteProjects = incompleteProjects.slice(0, 5);

      const ctx = this.myChart.nativeElement.getContext('2d');
      this.chart = new Chart(ctx, {
        type: 'bar',
        data: {
          labels: topIncompleteProjects.map(project => project.name),
          datasets: [{
            data: topIncompleteProjects.map(project => project.percentage),
            backgroundColor: '#FF9551',
            borderColor: '#FF9551',
            borderWidth: 2
          }]
        },
        options: {
          indexAxis: 'y',
          scales: {
            x: {
              beginAtZero: true,
              max: 100,
              title: {
                display: true,
                text: 'Completion Percentage'
              },
              ticks: {
                callback: function(value) {
                  return value + '%';
                }
              }
            },
            y: {
              title: {
                display: true,
                text: 'Project Names'
              }
            }
          },
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            title: {
              display: true,
              text: 'Top 5 Incomplete Projects by Completion Percentage',
              font: {
                size: 18
              }
            },
            legend: {
              display: false
            },
            tooltip: {
              callbacks: {
                label: function(context) {
                  return context.raw + '%';
                }
              }
            }
          }
        }
      });

    
  }
}
