export interface UpdateProject {
    members : number[],
    projectRoles : number[],
    title : string,
    stateId : number,
    priorityId : number,
    description :string,
    start : Date,
    end : Date,
    spent : number,
    percentage : number
}
