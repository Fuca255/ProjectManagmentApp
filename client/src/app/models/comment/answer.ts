import { User } from "../user/user";

export interface Answer {
    id: number,
    content: string,
    postTime: Date,
    editedTime: Date,
    commentId: number,
    user: User,
    editOpened : boolean,
    oldContent: string
}
