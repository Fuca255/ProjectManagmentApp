import { Component, OnInit, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Team } from '../../models/team';
import { User } from '../../models/user/user';
import { TeamService } from '../../services/team.service';
import { SpinnerIcon } from 'primeng/icons/spinner';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-edit-team',
  templateUrl: './edit-team.component.html',
  styleUrl: './edit-team.component.css'
})
export class EditTeamComponent implements OnInit{
  data = inject(MAT_DIALOG_DATA);
  dialogRef = inject(MatDialogRef);
  private spinner = inject(NgxSpinnerService);
  private team_service = inject(TeamService);
  team!: Team;
  users: User[] = [];
  selectedUsers: User[] = [];
  public updateTeamObj = {
    name : "",
    type : "",
    createdBy : 0,
    members : [1]
  }
  ngOnInit(): void {
    this.team = this.data[1];
    this.users = this.data[0];
    // console.log(this.team.members);
    this.team.members.forEach(element => {
      element.fulname = element.name + " " + element.lastname;
    });
    this.selectedUsers = this.team.members;
    // console.log(this.selectedUsers);
    this.updateTeamObj.name=this.team.name;
    this.updateTeamObj.type = this.team.type;
    this.updateTeamObj.createdBy = this.team.createdBy;
  }

  updateTeamRequest()
  {
    this.spinner.show();
    if(this.selectedUsers.length <= 0)
    {
      this.spinner.hide();
      // alert("Team does not have any memebers selected!");
      return;
    }
    if(this.updateTeamObj.name==="")
    {
      this.updateTeamObj.name = this.team.name;
    }
    this.updateTeamObj.members = [];
    this.selectedUsers.forEach(element => {
      this.updateTeamObj.members.push(element.id);
    });

    this.team_service.updateTeam(this.updateTeamObj,this.team.id).subscribe({
      next: (team:Team)=>{
        this.spinner.hide();
        this.dialogRef.close(team);
      }
    });
  }
}
