import { Component, OnInit, inject } from '@angular/core';
import { Role } from '../../models/role';
import { RoleService } from '../../services/role.service';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user/user';
import { NgxSpinnerService } from 'ngx-spinner';
import { SearchService } from '../../services/search.service';
import { UserFilter } from '../../models/user-filter';
import { UpdateUser } from '../../models/user/update-user';
import { NotificationService } from '../../services/notification.service';
import { CreateNotification } from '../../models/notifications/create-notification';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent implements OnInit {
  
  public roles : Role[] = [];
  public users : User[] = [];
  private role_service = inject(RoleService);
  private spinner = inject(NgxSpinnerService);
  private user_service = inject(UserService);
  private notification_service = inject(NotificationService);
  initialRoles: Map<number, Role> = new Map<number, Role>();
  initialActivation: Map<number, boolean> = new Map<number, boolean>();

  filter: UserFilter ={
    searchUser : ""
  }
  register: boolean = false;
  members: boolean = true;
  
  ngOnInit(): void {
    this.spinner.show();
    this.role_service.getAllRoles().subscribe({
      next : (roles: Role[])=> 
      {
        this.roles = roles.filter(r=>r.id!=5);
        this.spinner.hide();
      }
    });
    this.loadUsers();
  }

  showRegister(): void {
    this.register = true;
    this.members = false;
  }

  showMembers(): void {
    this.members = true;
    this.register = false;
  }

  // searchUsers(event: Event) {
  //   const query = (event.target as HTMLInputElement).value;
  //   this.search_service.changeSearchQuery(query);
  // }

  getFilter(filter: UserFilter)
  {
    this.filter.isActivatedFilter = filter.isActivatedFilter;
    this.filter.roleFilter = filter.roleFilter;
    this.loadUsers();
  }

  loadUsers()
  {
    this.spinner.show();
    this.user_service.getUsers(this.filter).subscribe({
      next : (users: User[]) => {
        this.users = users;
          this.spinner.hide();
          this.users = this.users.filter(u=>u.role.id!==5);
          this.users.forEach(user => {
            if(user.profilePicture)
              user.profilePicturePath = `data:${user.pictureType};base64,${user.profilePicture}`;
            else
              user.profilePicturePath = "../../../assets/pictures/defaultpfp.svg";
            this.initialRoles.set(user.id, user.role);
            this.initialActivation.set(user.id,user.isActivated);
          });
        }
    });
  }



  updateUserRole(role: Role,user : User)
  {
      user.role = role;
  }

  changeActivationStatus(activation : number, user:User)
  {
    
    if(activation==1)
      user.isActivated = true;
    else
      user.isActivated = false;

  }

  sendUpdateRequest(user: User){
    this.spinner.show();
    let body : UpdateUser = {
      id: user.id,
      name: user.name,
      lastname: user.lastname,
      username: user.username,
      email: user.email,
      jobTitle: user.jobTitle,
      organisation: user.organisation,
      department: user.department,
      roleId: user.role.id,
      roleName: user.role.name,
      isActivated: user.isActivated,
    }

    this.user_service.updateUser(body).subscribe({
      next: (updatedUser: User) => {
        if(!this.isDefaultRole(updatedUser))
        {

          let create :CreateNotification = {
            userIds: [updatedUser.id],
            title: 'Application Role Changed',
            description: `Your role on application has been changed to: '${updatedUser.role.name}' !`
          }
        this.notification_service.sendNotifcation(create).then(()=>{
          // console.log("message sent");
        }).catch((err)=>{console.log(err)})
        }
        user = updatedUser;
        this.initialRoles.set(user.id, user.role);
        this.initialActivation.set(user.id,user.isActivated);
        this.spinner.hide();
      },
      error: (err) => console.log(err)
    });
  }

  isDefaultRole(user: User): boolean {
    const initialRole = this.initialRoles.get(user.id);
    return initialRole ? initialRole.id === user.role.id : false;
  }
  
  isDefaultActivation(user: User): boolean {
    const initialActivate = this.initialActivation.get(user.id);
    return initialActivate !== undefined ? initialActivate === user.isActivated : false; 
  }
}
