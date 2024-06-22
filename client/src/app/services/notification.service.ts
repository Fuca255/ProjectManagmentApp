import { Injectable, inject } from '@angular/core';
import * as signalr from '@microsoft/signalr'
import { environment } from '../environments/environment';
import { CreateNotification } from '../models/notifications/create-notification';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Notification } from '../models/notifications/notification';
@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  //konekcija na hub sa beka
  public connection:signalr.HubConnection = new signalr.HubConnectionBuilder()
  .withUrl(this.baseUrl +"/notificationHub")
  .configureLogging(signalr.LogLevel.Information)
  .build();

  constructor() {
    // this.startConnection();
   }

  public updateNotification(user_id:number, notif_id:number):Observable<Notification>
  {
    const url = this.baseUrl + "/Notification/Update";
    return this.http.put<Notification>(url,{userId:user_id,notificationId:notif_id});
  }

  //konektovanje na hub

  public async startConnection(){
    try{

      await this.connection.start();
    } catch(error)
    {
      console.log("Ne uspesna konekcija");
      console.log(error);
    }
  }
  //prijava na hub
  public async login(userId: number)
  {
    // console.log(userId);
    
    // const connectionObject : Connection = {
    //   UserId: userId,
    //   Room: 'asd'
    // }
    return this.connection.invoke("Login",{userId: userId});
  }

  //slanje notifikacija
  public async sendNotifcation(createNotification: CreateNotification)
  {
    // console.log(createNotification);
    // this.connection.send("sendNotification")
    return this.connection.invoke("SendNotification",createNotification);
    
  }
  //odjava sa huba
  public async logout()
  {
    //odjava sa listenera
    this.connection.off("ReceiveMessage");
    return this.connection.stop();
  }
}
