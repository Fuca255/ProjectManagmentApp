import { Task } from "../task/task";

export interface TaskPaginatedObject {
    assignments: Task[],
    maxAssignments: number
}
