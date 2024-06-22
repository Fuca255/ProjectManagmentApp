import { Priority } from "../priority/priority"
import { State } from "../state/state"
import { User } from "../user/user"

export interface Project {
    id : number, 
    title : string,
    start : Date, 
    end : Date,
    state : State,
    budget : number,
    spent : number,
    type : string,
    percentage : number,
    description : string,
    priority : Priority,
    users : User[],
    ownerId: number,
    assignmentCount : number,
    truncatedTitle : string,
    truncatedDescription : string
}
