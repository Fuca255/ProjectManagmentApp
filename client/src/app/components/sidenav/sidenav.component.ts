import { Component, Output, EventEmitter, OnInit, HostListener, inject } from '@angular/core';
import { navbarData } from './nav-data';
import { animate, keyframes, style, transition, trigger } from '@angular/animations';
import { faSlack, faUncharted } from '@fortawesome/free-brands-svg-icons';
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { jwtDecode } from 'jwt-decode';


interface SideNavToggle {
  screenWidth: number;
  collapsed: boolean;
}



@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrl: './sidenav.component.css',
  animations:[
    trigger('fadeInOut',[
      transition(":enter", [
        style({opacity:0}),
        animate('350ms',
        style({opacity:1})
        )
      ]),
      transition(":leave", [
        style({opacity:1}),
        animate('350ms',
        style({opacity:0})
        )
      ])
    ]),
    trigger('rotate',[
      transition(':enter',[
        animate('1000ms',
        keyframes([
          style({transform: 'rotate(0deg)', offset: '0'}),
          style({transform: 'rotate(2turn)', offset: '1'})
        ]))
      ])
    ])
  ]
})
export class SidenavComponent implements OnInit {

  faDashboard = faSlack;


  @Output() onToggleSideNav: EventEmitter<SideNavToggle> = new EventEmitter();

  collapsed = false;
  screenWidth = 0;
  navData = navbarData;

  @HostListener('window:resize', ['$event'])
  onResize(event:any){
    this.screenWidth = window.innerWidth;
    if(this.screenWidth <= 768) {
      this.collapsed = !this.collapsed;
      this.onToggleSideNav.emit({collapsed: this.collapsed, screenWidth: this.screenWidth});
    }
  }
  private jwt_decode = inject(JwtDecoderService);
  
  ngOnInit(): void {
    // this.screenWidth = window.innerWidth;
    this.navData = [];
    let user = this.jwt_decode.getLoggedUser();
    if(user!==null)
    {
      let nav = navbarData;
      if(user.role==="Project Manager")
      {
        for (let i = 1; i < nav.length; i++) {
          const element = nav[i];
          this.navData.push(element);
          
        }
      }
      else if(user.role==="Admin")
      {
        this.navData = [nav[0],nav[5]];
      }
      else
        this.navData = [nav[2],nav[3],nav[5]];

        // console.log(this.navData);
    }
  }

  toggleCollapse(): void{
    this.collapsed = !this.collapsed;
    this.onToggleSideNav.emit({collapsed: this.collapsed, screenWidth: this.screenWidth});
  }

  closeSidenav(): void {
    this.collapsed = false;
    this.onToggleSideNav.emit({collapsed: this.collapsed, screenWidth: this.screenWidth});
  }
}
