import { Component, Inject, OnInit, inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { UserService } from '../../services/user.service';
import { ProjectService } from '../../services/project.service';
import { Project } from '../../models/project/project';
import { PriorityService } from '../../services/priority.service';
import { TeamService } from '../../services/team.service';
import { User } from '../../models/user/user';
import { CreateProject } from '../../models/project/create-project';
import { Team } from '../../models/team';
import { Role } from '../../models/role';
import { RoleService } from '../../services/role.service';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { CreateNotification } from '../../models/notifications/create-notification';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-create-project-overlay',
  templateUrl: './create-project-overlay.component.html',
  styleUrl: './create-project-overlay.component.css'
})
export class CreateProjectOverlayComponent implements OnInit {
  private jwtService = inject(JwtDecoderService);
  private notification_service =  inject(NotificationService);
  loggedInUserId: number | null = null;
  fullname!: string;
  roleid!: number;
  userid!: number;

  users: User[] = [];
  priorities: any[] | undefined;
  teams: Team[] = [];
  roles: Role[] = [];
  filteredRoles: Role[] = [];
  showDropdown: boolean = false;
  openDropdownUpwards: boolean = false;
  hoveredTeam: any;
  submitted = true;
  submissionError: string | null = null;

  projectObj!: CreateProject;

  selectedUserIds: number[] = [];


  selectedUserRolesMap : Map<number,number> = new Map<number,number>();
  userRoles: Map<number, number> = new Map<number, number>();
  defaultRoles: Map<number, number> = new Map<number, number>();

  selectedPriorityId!: number;
  private spinner = inject(NgxSpinnerService);

  searchQuery: string = '';
  selectedUsers: User[] = [];

  selectedCurrency: string = 'USD';
  currencies: string[] = ['AUD', 'BAM', 'CAD', 'CHF', 'CNY', 'EUR', 'GBP', 'JPY', 'MKD', 'RON', 'RSD', 'RUB', 'USD'];

  currencyRates: { [key: string]: number } = {
    USD: 1,
    RSD: 0.0093,
    EUR: 1.09,
    GBP: 1.27,
    JPY: 0.0064,
    CAD: 0.73,
    AUD: 0.67,
    CHF: 1.11,
    CNY: 0.14,
    BAM: 0.56,
    MKD: 0.018,
    RON: 0.22,
    RUB: 0.011
  };
  constructor(private dialogRef: MatDialogRef<CreateProjectOverlayComponent>, private userService: UserService, private projectService: ProjectService, private priorityService: PriorityService, private teamService: TeamService, private roleService: RoleService) { }

  ngOnInit(): void {
    let token = this.jwtService.getToken();
        if (token != null) {
            let decode = this.jwtService.decodeToken(token);
            this.fullname = decode.fullname;
            this.loggedInUserId = decode.user_id;
            this.roleid = decode.role_id;
            this.userid = decode.user_id;
        }
    this.roleService.getAllRoles().subscribe(roles => {
       this.roles = roles;
    });
    this.spinner.show();
    this.userService.getUsers().subscribe(users => {
      users = users.filter(u=>u.isActivated);
      this.users = users;

      this.users.forEach(user => {
          this.userRoles.set(user.id, user.role.id);
          this.defaultRoles = this.userRoles;
      });
      this.spinner.hide();
    });
    this.priorityService.getPriorities().subscribe(priorities => {
      this.priorities = priorities;
    });
    this.teamService.getMyCreatedTeams(this.userid).subscribe(teams => {
      this.teams = teams;
    });
    this.projectObj = {
      userIds : [Number(this.userid)],
      userProjectRoleIds : [],
      priorityId : this.selectedPriorityId,
      title : "",
      start : new Date(),
      end : new Date(),
      budget : 0,
      description : "",
      type : "neki tip",
      ownerId : 0,
    };

    this.userService.getUserById(this.userid).subscribe(user =>{
      this.selectedUsers[0] = user;
    });
  }

  //-----------------

  titleTouched: boolean = false;
  budgetTouched: boolean = false;
  priorityTouched: boolean = false;
  startDateTouched: boolean = false;
  endDateTouched: boolean = false;

  //----------------

  toggleDropdown(): void {
    this.showDropdown = !this.showDropdown;
    if (this.showDropdown) {
      setTimeout(() => {
        const dropdown = document.querySelector('.options') as HTMLElement;
        if (dropdown) {
          const rect = dropdown.getBoundingClientRect();
          const viewportHeight = window.innerHeight || document.documentElement.clientHeight;
          this.openDropdownUpwards = (rect.bottom > viewportHeight);
        }
      }, 0);
    } else {
      this.searchQuery = '';
    }
  }

  onUserSelected(user: User, event: any): void {
    event.stopPropagation();
    const target = event.target;
    if (target && target.value !== undefined) {
      const selectedRoleId = event.target?.value;
      if (this.isSelected(user)) {
          this.selectedUserRolesMap.delete(user.id);
      } else {
          this.selectedUserRolesMap.set(user.id,selectedRoleId);
      }
    }
}

  isSelected(user: User): boolean {
    if(user.id == this.userid)
      return true;
    return this.selectedUserRolesMap.has(user.id);
  }

  toggleUserSelection(user: User): void {
    const selectedRoleId = this.userRoles.get(user.id);
    if (this.isSelected(user)) {
        this.selectedUserRolesMap.delete(user.id);
        this.selectedUsers = this.selectedUsers.filter(selectedUser => selectedUser.id !== user.id);
    } else {
        this.selectedUserRolesMap.set(user.id, selectedRoleId!);
        this.selectedUsers.push(user);
    }
  }

  onRoleChanged(user: User, event: any) {
    const selectedRoleId = event.target.value;
    this.userRoles.set(user.id,selectedRoleId);

    if(this.isSelected(user))
      this.selectedUserRolesMap.set(user.id,selectedRoleId);
  }
  


  closeOverlay(): void {
      this.dialogRef.close();
  }

  createProject(): void {
    this.projectObj.budget = this.convertToDollars(this.projectObj.budget, this.selectedCurrency);
    let selected_users : number[] = [];
    let selected_roles : number[] = [];
    this.selectedUserRolesMap.forEach((value,key) => {
      selected_roles.push(value);
      selected_users.push(Number(key));
    });
    // console.log(selected_users);
    this.projectObj.userProjectRoleIds = selected_roles;
    this.spinner.show();
    this.projectObj.userIds = selected_users;
    this.projectObj.priorityId = this.selectedPriorityId;
    this.projectObj.ownerId = this.userid;
    this.submissionError = null;

    if (this.loggedInUserId != null) {
      //this.selectedUserRolesMap.set(this.loggedInUserId, this.roleid);
      this.projectObj.userIds.push(this.loggedInUserId);
      this.projectObj.userProjectRoleIds.push(this.roleid);
    }

    console.log(this.projectObj.start);
    if (!this.projectObj.title.trim() || !this.projectObj.priorityId || !this.projectObj.start || !this.projectObj.end || (this.projectObj.end == this.projectObj.start) || (this.projectObj.end < this.projectObj.start) || (this.projectObj.budget === null || this.projectObj.budget === undefined || this.projectObj.budget < 0)) {
      this.submissionError = 'Please fill in all necessary fields.';
      this.spinner.hide();
      return;
    }
    this.projectObj.ownerId = this.userid;
    this.projectService.createProject(this.projectObj).subscribe(response => {
      // alert('Project created successfully!');
      this.spinner.hide();
      let asd: number[]  = []
      response.users.forEach(user => {
        asd.push(Number(user.id))
      });
      // console.log(response.users);
      // console.log(asd);
      if(selected_users.length > 0)
      {
        let create :CreateNotification = {
          userIds: asd,
          title: 'New project',
          description: `You have been added on project '${response.title}'`
        }
        this.notification_service.sendNotifcation(create).then(()=>{
          // console.log("message sent");
        }).catch((err)=>{console.log(err)})

      }

      // console.log("iz overlaya:",response);
      this.dialogRef.close(response);
      // this.resetForm();
      // this.selectedUsers = [];
      // this.userService.getUserById(this.userid).subscribe(user =>{
      //   this.selectedUsers[0] = user;
      // });
      // console.log(this.selectedUsers);
    }, error => {
      console.error('Error creating project:', error);
      this.spinner.hide();
      this.submissionError = 'Error creating project. Please try again.';
    });
  }

  resetForm(): void {
    this.projectObj.title = '';
    this.projectObj.priorityId = 0;
    this.projectObj.start = new Date();
    this.projectObj.end = new Date();
    this.projectObj.userIds = [];
    this.projectObj.ownerId = 0;
    this.projectObj.userProjectRoleIds = [];
    this.selectedUserIds = [];
    this.selectedUserRolesMap.clear();
    this.selectedPriorityId = 0;
    this.projectObj.budget = 0;
    this.projectObj.description = '';
    this.projectObj.type = '';
    this.userRoles = this.defaultRoles;
  }

  showTeamMembers(team: Team): void {
    this.hoveredTeam = team;
  }

  hideTeamMembers(event: MouseEvent): void {
    const target = event.relatedTarget as HTMLElement;

    if (target && target.closest('.team-option') === null && target.closest('.team-members') === null) {
        this.hoveredTeam = null; 
    }
  }

  getUserName(memberId: number): string {
    const user = this.users.find(user => user.id === memberId);
    if (user) {
        return `${user.name} ${user.lastname}`;
    } else {
        return '';
    }
  }

  onTeamSelected(team: Team): void {
    let team_member_ids: User[] = [];
    team.members.forEach(member => {
        team_member_ids.push(member);
    });
    if (this.isSelectedTeam(team.id)) {
        team_member_ids.forEach(member => {
            if (member.id != this.loggedInUserId) {
                this.selectedUserRolesMap.delete(member.id);
                this.selectedUsers = this.selectedUsers.filter(selectedUser => selectedUser.id !== member.id);
            }
        });
    } else {
        team_member_ids.forEach(member => {
            if (member.id != this.loggedInUserId) {
                const selectedRoleId = this.userRoles.get(member.id);
                if (!this.isSelected(member)) {
                    this.selectedUserRolesMap.set(member.id, selectedRoleId!);
                    this.selectedUsers.push(member);
                }
            }
        });
    }
  }

  isSelectedTeam(teamId: number): boolean {
      const team = this.teams.find((team: Team) => team.id === teamId);
      if (team) {
          return team.members.every((member: User) => this.isSelected(member));
      }
      return false;
  }

  getRolesForUser(user: User): Role[] {
    if (user.id == this.loggedInUserId) {
      return this.roles;
    } else {
      return this.roles.filter(role => role.id !== 1 && role.authority >= user.role.authority);
    }
  }

  get filteredUsers(): any[] {
    return this.users.filter(user =>
      (user.name + ' ' + user.lastname).toLowerCase().includes(this.searchQuery.toLowerCase()) && user.role.authority != 1
    );
  }

  get filteredTeams(): any[] {
      return this.teams.filter(team =>
          team.name.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
  }

  removeSelectedUser(user: User): void {
    this.selectedUsers = this.selectedUsers.filter(selectedUser => selectedUser.id !== user.id);
    this.selectedUserRolesMap.delete(user.id);
  } 

  getSelectedRoleId(userId: number): number | null {
    if(this.userRoles.get(userId) == 1 && userId != this.loggedInUserId && !this.selectedUserRolesMap.get(userId))
      this.userRoles.set(userId, 2);
    return this.selectedUserRolesMap.get(userId) || this.userRoles.get(userId) || null;
  }

  isEndDateBeforeStartDate(): boolean {
    if (this.projectObj.start && this.projectObj.end) {
      const startDate = new Date(this.projectObj.start);
      const endDate = new Date(this.projectObj.end);
      return startDate >= endDate;
    }
    return false;
  }

  convertToDollars(amount: number, currency: string): number {
    return parseFloat((amount * this.currencyRates[currency]).toFixed(2));
  }
}
