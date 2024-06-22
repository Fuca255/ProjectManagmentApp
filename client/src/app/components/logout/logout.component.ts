import { Component, inject } from '@angular/core';
import { AuthenticationService } from '../../services/atentication.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent {
  private authService = inject(AuthenticationService);
  private notif_serv = inject(NotificationService);

  logout()
  {
    this.notif_serv.logout();
    this.authService.logout();
  }
}
