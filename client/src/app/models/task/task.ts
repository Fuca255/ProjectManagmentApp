
import { Priority } from "../priority/priority";
import { State } from "../state/state";
import { TaskGroup } from "./task-group";
import { User } from "../user/user";

export interface Task{
    id : number,
    assignees : User[],
    title : string,
    owner : User,
    description : string,
    start : Date,
    end : Date,
    state : State,
    percentage : number,
    priority : Priority,
    taskGroup : TaskGroup,
    type : string,
    dummyTitle : string,
    timeDifference: number,
    remaining: string,
    endMilliseconds: number,
    startMilliSeconds: number,
    endSeconds: number,
    endMinutes: number,
    endHours: number,
    endYear: number,
    endMonth: number,
    endDate: number,
    startSeconds: number,
    startMinutes: number,
    startHours: number,
    startYear: number,
    startMonth: number,
    startDate: number,
    depndentOn: number[],
    isClosed : boolean
}