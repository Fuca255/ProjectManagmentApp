import { User } from "./user/user";

export interface Team {
    id : number,
    name : string,
    type : string,
    members : User[],
    createdBy: number,
    reducedTeam:string
}
