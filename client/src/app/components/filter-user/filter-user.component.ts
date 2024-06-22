import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { Role } from '../../models/role';
import { RoleService } from '../../services/role.service';
import { UserService } from '../../services/user.service';
import { UserFilter } from '../../models/user-filter';

@Component({
  selector: 'app-filter-user',
  templateUrl: './filter-user.component.html',
  styleUrl: './filter-user.component.css'
})
export class FilterUserComponent implements OnInit{
  private roleService = inject(RoleService);
  private userService = inject(UserService);

  @Output() sendFilterBack = new EventEmitter<UserFilter>();

  @Input() filter! : UserFilter;
  roles: Role[] = [];
  selectedRoles: Role[] = [];
  isActivated: number = 1;
  sortMode: number = -1;

  activationList : string[]= ["Select Activation","Activated","Deactivated"];
  selectedActivation:string = this.activationList[0];

  

  ngOnInit(): void {
    this.roleService.getAllRoles().subscribe(roles =>{
      this.roles = roles;
    });
  }

  sendFilter()
  {
    this.filter.roleFilter = [];
    this.selectedRoles.forEach(element => {
      this.filter.roleFilter?.push(element.id);
    });
    if(this.selectedActivation===this.activationList[1])
      this.filter.isActivatedFilter = 1;
    else if(this.selectedActivation===this.activationList[2])
      this.filter.isActivatedFilter = 0;
    else
      this.filter.isActivatedFilter = -1;

    this.sendFilterBack.emit(this.filter);
  }


  // applyFilters() {
  //   const filters = {
  //     roleFilter: this.selectedRoles.map(role => role.id),
  //     isActivatedFilter: [this.isActivated],
  //     sortFlag: this.sortMode
  //   };
  //   this.userService.getUsers(filters).subscribe(data => {
  //     this.userService.updateFilteredUsers(data);
  //   });
  // }
}
