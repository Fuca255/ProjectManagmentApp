import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { tick } from '@angular/core/testing';
import { Comment } from '../models/comment/comment';
import { Answer } from '../models/comment/answer';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  getAllComentsByTask(task_id:number) : Observable<Comment[]>
  {
    const url = this.baseUrl+"/Comment/getAllCommentsByAssignment/"+task_id;
    return this.http.get<Comment[]>(url);
  }

  createComment(data:any) : Observable<Comment>
  {
    const url = this.baseUrl + "/Comment/createComment";
    return this.http.post<Comment>(url,data);
  }
  createAnswer(data:any)
  {
    const url = this.baseUrl + "/Answer/CreateAnswer";
    return this.http.post<Answer>(url,data);
  }

  updateComment(data:any) : Observable<Comment>
  {
    const url = this.baseUrl + "/Comment/updateComment";
    return this.http.put<Comment>(url,data);
  }

  updateAnswer(data:any):Observable<Answer>
  {
    const url = this.baseUrl + "/Answer/updateAnswer";
    return this.http.put<Answer>(url,data);
  }
  deleteComment(comment_id:number)
  {
    const url =  this.baseUrl + "/Comment/deleteCommentById/"+comment_id;
    return this.http.delete(url);
  }
  deleteAnswer(answer_id:number)
  {
    const url = this.baseUrl + "/Answer/"+answer_id;
    return this.http.delete(url);
  }
}
