import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskCardKanbanComponent } from './task-card-kanban.component';

describe('TaskCardKanbanComponent', () => {
  let component: TaskCardKanbanComponent;
  let fixture: ComponentFixture<TaskCardKanbanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TaskCardKanbanComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TaskCardKanbanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
