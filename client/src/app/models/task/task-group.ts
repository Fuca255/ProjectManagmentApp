import { Task } from "./task";

export interface TaskGroup {
    id: number,
    title: string,
    projectId: number,
    parentTaskGroupId: number,
    children? : any[],
    assignments : Task[],
    dummyTitle?: string
}
