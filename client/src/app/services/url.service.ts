import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PreviousUrlService {

  private previousUrlSubject = new BehaviorSubject<string | null>(null);
  public previousUrl$ = this.previousUrlSubject.asObservable();

  setUrl(url: string | null) {
    this.previousUrlSubject.next(url);
  }

  getUrl(): string | null {
    return this.previousUrlSubject.getValue();
  }

}