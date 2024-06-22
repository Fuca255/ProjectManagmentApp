import { TaskCardKanbanComponent } from "../../components/task-card-kanban/task-card-kanban.component";

export class TaskKanban {
    constructor(
      public title: string,
      public description: string,
      public component: TaskCardKanbanComponent
    ) {}
  }
  