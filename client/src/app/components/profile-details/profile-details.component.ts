import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RoleService } from '../../services/role.service';
import { Role } from '../../models/role';
import { UserService } from '../../services/user.service';
import { UpdateUser } from '../../models/user/update-user';
import { Router } from '@angular/router';
import { UserProfileService } from '../../services/user-profile.service';
import { NgForm } from '@angular/forms';
import { NgToastService } from 'ng-angular-popup';
import { Notify } from '../../models/notifications/notify';
import { User } from '../../models/user/user';

@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrl: './profile-details.component.css'
})
export class ProfileDetailsComponent {
  @ViewChild('fileInput') fileInput!: ElementRef;
  user: UpdateUser;
  loggedUser!: User;
  originalUser: UpdateUser;
  roleId!: number;
  roles!: Role[];
  editMode = false;
  picture!: string;
  uploadingPicture: boolean = false;
  notify : Notify;
  usernameError: string = '';

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private roleService: RoleService, private userService: UserService, private router: Router, private userProfileService: UserProfileService,private toast : NgToastService,) {
    this.user = data.user;
    this.loggedUser = data.user;
    this.originalUser = { ...this.user };
    this.roleId = data.role;
    this.user.roleId = this.roleId;
    this.profilePicture(this.user.id);
    this.notify = new Notify(toast);
    this.usernameError = '';
  }

  ngOnInit(): void {
    this.roleService.getAllRoles().subscribe(roles => {
      this.roles = roles;
    });
  }

  toggleEditMode() {
    if (!this.editMode) {
      this.originalUser = { ...this.user };
    } else {
      this.user = { ...this.originalUser };
    }
    this.editMode = !this.editMode;
  }

  editProfile(userid: Number, form: NgForm): void {
    if (form.value.organisation == null || form.value.department == null) {
      form.value.organisation = '';
    }
    if (form.value.department == null) {
      form.value.department = '';
    }
    if (form.invalid) {
      return;
    }

    this.userService.updateUser(this.user).subscribe({
      next: (res) => {
        const response = res as any as { message: string };
        if(response.message == "This username already exists in the database.") {
          this.usernameError = "*This username already exists.";
          this.notify.showWarn("Profile edited", "Profile form not filled correctly!");
          this.loggedUser = res;
          return;
        }
        
        this.userProfileService.setUserProfile(this.user);
        this.notify.showSuccess("Profile edited","Profile edited successfully!")
        this.originalUser = { ...this.user };
      },
      error: (err) => {
        this.notify.showWarn("Profile edited", "Profile form not filled correctly!");
      }
    });
    this.usernameError = "";
  }

  onFileSelected(event: any) {
    this.uploadingPicture = true;
    const file: File = event.target.files[0];
    let picture = {
      data: '',
      filename: '',
      type: ''
    }
    picture.type = file.type;
    picture.filename = file.name;
    if (file && file.type.startsWith('image/')) {
      const reader = new FileReader();

      reader.onload = () => {
        const base64String = (reader.result as string).split(',')[1];
        picture.data = base64String;
        this.uploadProfilePicture(this.user.id, picture);
      };

      reader.readAsDataURL(file);
    } else {

      this.notify.showWarn("Image file not valid!","Please select a valid image file.")
      // alert('Please select a valid image file.');

      // alert('Please select a valid image file.');
      this.fileInput.nativeElement.value = '';
      this.uploadingPicture = false;

    }
  }

  uploadProfilePicture(userId: number, base64String: any) {
    this.uploadingPicture = true;
    this.userService.uploadProfilePicture(userId, base64String).subscribe({
      next: (message: string) => {
        this.profilePicture(userId);
        this.uploadingPicture = false;
      },
      error: (err) => { this.uploadingPicture = false; }
    });
  }

  profilePicture(userId: number) {
    this.userService.profilePicture(userId).subscribe({
      next: (message: { profilePicture: string, type: string }) => {
        if (message.profilePicture) {
          this.picture = `data:${message.type};base64,${message.profilePicture}`;
          this.userService.setProfilePicture(this.picture);
        } else {
          this.picture = "../../../assets/pictures/defaultpfp.svg";
        }
      }, error: (err) => {
        this.notify.showError("Error retrieving profile picture:", err)
        console.error('Error retrieving profile picture:', err);
      }
    });
  }
}
