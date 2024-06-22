import { Task } from "./task/task";
import { TaskGroup } from "./task/task-group";

export interface ListFlatNode {
    expandable: boolean,
    node : any,
    level: number,
}
