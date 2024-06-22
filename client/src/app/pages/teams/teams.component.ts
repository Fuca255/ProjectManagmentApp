import { Component, OnInit, inject } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { TeamService } from '../../services/team.service';
import { Team } from '../../models/team';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user/user';
import { MatDialog } from '@angular/material/dialog';
import { EditTeamComponent } from '../../components/edit-team/edit-team.component';
import { textAlign } from 'html2canvas/dist/types/css/property-descriptors/text-align';
import { ChangeDetectorRef } from '@angular/core';
import { Notify } from '../../models/notifications/notify';
import { NgToastService } from 'ng-angular-popup';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrl: './teams.component.css'
})
//updae i create
// "name": "string",
// "type": "string",
// "members": [
//   0
// ]
export class TeamsComponent implements OnInit {

  private spinner = inject(NgxSpinnerService);
  private jwt_service = inject(JwtDecoderService);
  private team_service = inject(TeamService);
  private user_service = inject(UserService);
  private dialog = inject(MatDialog);
  public searchText = "";

  notify : Notify

  private loggedUser:any;
  public createTeam = {
    name : "",
    type : "",
    createdBy : 0,
    members : [1]
  }
  public users: User[]= [];
  public selectedUsers: User[] =  [];
  public teams : Team[] = [];

  constructor(private cdr: ChangeDetectorRef, private toast : NgToastService) {
    this.notify = new Notify(toast)
  }

  ngOnInit(): void {
    this.spinner.show();
    this.loggedUser = this.jwt_service.getLoggedUser();
    this.createTeam.createdBy = this.loggedUser.user_id;
    this.loadTeams();
    this.user_service.getUsers().subscribe({
      next :(users: User[])=>{
        let pom: User[] = []
        users = users.filter(u=>u.isActivated);
        users.forEach(element => {
          element.fulname = element.name + " " + element.lastname;
          if(element.id!==Number(this.loggedUser.user_id) && element.role.name!=="Admin")
          pom.push(element);
        });
        this.users = pom;
        this.spinner.hide();
      }
    });
  }

  createTeamRequest()
  {
    if(this.selectedUsers.length <= 0 || this.createTeam.name==="")
    {
      this.notify.showWarn("Create team", "Create form not filled correctly!")
      return;
    }
    this.spinner.show();
    this.createTeam.members = [];
    this.selectedUsers.forEach(element => {
      this.createTeam.members.push(element.id);
    });
    this.team_service.createTeam(this.createTeam).subscribe({
      next: (team: Team) =>{
        this.teams.push(team);
        this.createTeam.name = "";
        this.selectedUsers = [];
        this.notify.showSuccess("Create team", "Team created successfuly!")
        this.loadTeams();
      }
    })
    
  }

  openEditOverlay(team: Team)
  {
    const dialogRef = this.dialog.open(EditTeamComponent,
      {data : [this.users,team]}
    )
    dialogRef.afterClosed().subscribe(result=>{
      let index = this.teams.findIndex(t=>t.id==result.id);
      this.teams.splice(index,1,result);
      this.loadTeams();
    });
  }

  loadTeams()
  {
    // this.spinner.show();
    // console.log(this.searchText);
    this.team_service.getMyCreatedTeams(this.loggedUser.user_id,this.searchText).subscribe({
      next :(teams: Team[]) =>{
        this.teams = teams;
        this.reduceTeamMembers();
        this.spinner.hide();
      }
    });
  }
  deleteTeam(team:Team)
  {
    let response = confirm(`Team ${team.name} will be deleted permanently.Do you wish to proceed?`);
    if(response)
    {

      let index = this.teams.findIndex(t=>t.id===team.id);
      this.teams.splice(index,1);
      this.team_service.deleteTeam(team.id).subscribe({
        next: (res)=>{
          this.notify.showSuccess("Delete team", "Team deleted successfuly!")
        },
        error: (err)=>{console.log(err);}
      });

  }
  }
  

  reduceTeamMembers(){
    
    for (let i = 0; i < this.teams.length; i++) {
      let assigneesString = ""
      let realAssignessStr = ""
      const team = this.teams[i];
      if(team!==undefined)
        {
          assigneesString = team.members[0].name + " "+ team.members[0].lastname;
          realAssignessStr = team.members[0].name + " "+ team.members[0].lastname;
          for (let index = 1; index < team.members.length; index++) {
            const element = team.members[index];
            if(index < 3)
              assigneesString+=", " + element.name + " "+element.lastname;
            
            realAssignessStr+=", " + element.name + " "+element.lastname;
  
          }
          if(team.members.length >= 4)
            assigneesString+=" +"+(team.members.length - 2);

          team.reducedTeam = assigneesString;
        }
      
    }
  }

}
