import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './services/auth.interceptor';
import { AuthenticationService } from './services/atentication.service'; 


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { ReglogComponent } from './components/reglog/reglog.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { ProjectsComponent } from './pages/projects/projects.component';
import { TasksComponent } from './pages/tasks/tasks.component';
import { ActivityComponent } from './pages/activity/activity.component';
import { TeamsComponent } from './pages/teams/teams.component';
import { HelpcentreComponent } from './pages/helpcentre/helpcentre.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ProjectPreviewComponent } from './components/project-preview/project-preview.component';
import { ProjectTaskComponent } from './components/project-task/project-task.component';
import { AuthComponent } from './pages/auth/auth.component';
import { ProjectStatusComponent } from './components/project-status/project-status.component';
import { MyTooltipDirective } from './directives/my-tooltip.directive';
import { CreateProjectOverlayComponent } from './components/create-project-overlay/create-project-overlay.component';
import { ProjectWeeklyAnalizeComponent } from './components/project-weekly-analize/project-weekly-analize.component';
import { TaskTrackComponent } from './components/task-track/task-track.component';
import { TaskTagsComponent } from './components/task-tags/task-tags.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { HeaderComponent } from './components/header/header.component';
import { KanbanComponent } from './components/kanban/kanban.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ProjectDetailsComponent } from './pages/project-details/project-details.component';
import { TaskCardKanbanComponent } from './components/task-card-kanban/task-card-kanban.component';
import { AddTaskComponent } from './components/add-task/add-task.component';
import { TaskHeaderComponent } from './components/task-header/task-header.component';
import { ProjectHeaderComponent } from './components/project-header/project-header.component';
import { GantogramComponent } from './components/gantogram/gantogram.component';
import { AdminComponent } from './pages/admin/admin.component';
import { LogoutComponent } from './components/logout/logout.component';
import { TaskDetailsComponent } from './pages/task-details/task-details.component';
import { EditProjectOverlayComponent } from './components/edit-project-overlay/edit-project-overlay.component';
import { MatSliderModule } from '@angular/material/slider';
import {NgxSpinnerModule} from 'ngx-spinner'

import { ThyLayoutModule } from 'ngx-tethys/layout';
import { ThyNavModule } from 'ngx-tethys/nav';
import { ThyButtonModule } from 'ngx-tethys/button';
import { ThyCheckboxModule } from 'ngx-tethys/checkbox';
import { ThySwitchModule } from 'ngx-tethys/switch';
import { ThyNotifyModule } from 'ngx-tethys/notify';
import { ThyDatePickerModule } from 'ngx-tethys/date-picker';


import {SliderModule} from 'primeng/slider'
import {MultiSelectModule} from 'primeng/multiselect'
import {CalendarModule} from 'primeng/calendar'
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';

import { NgxGanttModule } from '@worktile/gantt';
import { RouterModule } from '@angular/router';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { ProfileDetailsComponent } from './components/profile-details/profile-details.component';
import { SortDetailsComponent } from './components/sort-details/sort-details.component';
import { SortProjectComponent } from './components/sort-project/sort-project.component';


import { MatTooltipModule } from '@angular/material/tooltip';  
import { MatButtonModule } from '@angular/material/button';

import {NgToastModule} from 'ng-angular-popup'
import { MatChipsModule } from '@angular/material/chips';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ProjectFilterComponent } from './components/project-filter/project-filter.component';
import { TaskFilterComponent } from './components/task-filter/task-filter.component';
import { TaskListComponent } from './components/task-list/task-list.component';
import { MatTreeModule } from '@angular/material/tree';
import { TaskListTaskComponent } from './components/task-list-task/task-list-task.component';
import { AddTaskGroupComponent } from './components/add-task-group/add-task-group.component';
import { EditTeamComponent } from './components/edit-team/edit-team.component';
import { FilterUserComponent } from './components/filter-user/filter-user.component';
import { ResetpasswordComponent } from './pages/resetpassword/resetpassword/resetpassword.component';

import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { BadgeModule} from 'primeng/badge';
import { QuillModule } from 'ngx-quill'

@NgModule({
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ],
  declarations: [
    AppComponent,
    AuthComponent,
    LoginComponent,
    RegisterComponent,
    ReglogComponent,
    SidenavComponent,
    DashboardComponent,
    ProjectsComponent,
    TasksComponent,
    ActivityComponent,
    TeamsComponent,
    HelpcentreComponent,
    ProjectPreviewComponent,
    ProjectTaskComponent,
    ProjectStatusComponent,
    MyTooltipDirective,
    CreateProjectOverlayComponent,
    ProjectWeeklyAnalizeComponent,
    TaskTrackComponent,
    TaskTagsComponent,
    HeaderComponent,
    KanbanComponent,
    ProjectDetailsComponent,
    TaskCardKanbanComponent,
    AddTaskComponent,
    TaskHeaderComponent,
    ProjectHeaderComponent,
    GantogramComponent,
    LogoutComponent,
    TaskDetailsComponent,
    AdminComponent,
    EditProjectOverlayComponent,
    ProfileDetailsComponent,
    SortDetailsComponent,
    SortProjectComponent,
    ProjectFilterComponent,
    TaskFilterComponent,
    TaskListComponent,
    TaskListTaskComponent,
    AddTaskGroupComponent,
    EditTeamComponent,
    FilterUserComponent,
    ResetpasswordComponent,
  ],
  imports: [
    FormsModule,
    QuillModule.forRoot(),
    BrowserModule,
    SliderModule,
    BadgeModule,
    CalendarModule,
    ProgressSpinnerModule,
    MultiSelectModule,
    DropdownModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FontAwesomeModule,
    DragDropModule,
    MatSliderModule,
    NgxSpinnerModule,
    NgxGanttModule,
    RouterModule.forRoot([]),
    ThyButtonModule,
    ThyNavModule,
    ThyLayoutModule,
    ThyCheckboxModule,
    ThyNotifyModule,
    ThySwitchModule,
    ThyDatePickerModule,
    MatButtonToggleModule,
    MatIconModule,
    MatTooltipModule,
    MatButtonModule,
    MatIconModule,
    MatTreeModule,
    NgToastModule,
    MatChipsModule,
    NgbModule,
    InputTextModule,
  ],
  providers: [
    provideClientHydration(),
    provideAnimationsAsync(),
    AuthenticationService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
