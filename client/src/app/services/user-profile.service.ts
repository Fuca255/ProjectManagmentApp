import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { UpdateUser } from '../models/user/update-user';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  public userProfileSubject = new BehaviorSubject<UpdateUser | null>(null);

  getUserProfile(): UpdateUser | null {
    return this.userProfileSubject.getValue();
  }

  setUserProfile(user: UpdateUser): void {
    this.userProfileSubject.next(user);
  }
}
