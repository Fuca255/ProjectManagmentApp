export interface CreateProject {
    userIds : number[],
    userProjectRoleIds : number[],//ovo je za id-ove project rola
    priorityId : number,
    title : string,
    start : Date,
    end : Date,
    budget : number,
    description : string,
    type : string,
    ownerId : number
}
