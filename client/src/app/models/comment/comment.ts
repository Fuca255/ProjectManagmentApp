import { Answer } from "./answer";
import { User } from "../user/user";

export interface Comment {
    id : number,
    content : string,
    editedTime : Date,
    postTime : Date,
    userId : number,
    user : User,
    answers : Answer[],
    replyOpened : boolean,
    answerContent : string,
    oldContent: string,
    editOpened : boolean
}
