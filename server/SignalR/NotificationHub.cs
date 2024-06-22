using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using server.DTOs.Notification;
using server.Mappers;
using server.Models;

namespace server.SignalR
{
    public class NotificationHub : Hub
    {
        
        private readonly IDictionary<string, UserRoomConnection > _connections;
        private readonly INotificationRepository _note_repo;
        private readonly IUserRepository _user_repo;
        public NotificationHub(IDictionary<string, UserRoomConnection> connections, INotificationRepository note_repo, IUserRepository user_repo)
        {
            _connections = connections;
            _user_repo = user_repo;
            _note_repo = note_repo;
        }
        public async Task Login(UserRoomConnection userRoomConnection){
            // await Groups.AddToGroupAsync(Context.ConnectionId,"App");
            var con = _connections.FirstOrDefault(x => x.Value.UserId == userRoomConnection.UserId);
            if (con.Key == null)
                _connections[Context.ConnectionId] = userRoomConnection;
            //ime na koju ce da se osluskuje sa fronta, ostalo su parametri, ovde getuj sve notifikacije iz baze i posalji na front
            //prilikom uspostavljanja konekcije posalji sve notifikacije za ovog usera
            var notifications = await _note_repo.GetAllNotificationsByUserAsync(userRoomConnection.UserId);
            foreach (var notification in notifications)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", "Server", notification.Notification.toNotificationDto(notification.MarkAsRead));                
            }
            
            // await SendConnectedUser("App");
        }

        public async Task SendNotification(CreateNotificationDto createNotification)//stavi notifikaciju
        {
            // if(_connections.TryGetValue(Context.ConnectionId, out UserRoomConnection userRoomConnection)) // da li je user koji poziva ovo logovan
            // {
                var notification = createNotification.fromCreateDtoToNotificationDto();
                List<User> users = new List<User>();
                System.Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                foreach (var userId in createNotification.UserIds)
                {
                    var user = await _user_repo.GetUserByIdAsync(userId);
                    if(user==null)
                        return;
                    users.Add(user);
                }
                notification = await _note_repo.CreateNotification(notification,users);
                foreach (var conn in _connections)
                {
                    System.Console.WriteLine(_connections.Count+" "+users.Count);
                    var user = users.FirstOrDefault(u=>u.Id == conn.Value.UserId);
                    if(user!=null)
                    {
                        System.Console.WriteLine("Usao sam na send");
                        await Clients.Client(conn.Key).SendAsync("ReceiveMessage",conn.Value.UserId,notification.toNotificationDto(false));
                    }
                    System.Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");                 
                }
                // await Clients.Group("App").SendAsync("ReceieveMessage",userRoomConnection.UserId, notification);

        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exp)
        {
            // if(!_connections.TryGetValue(Context.ConnectionId,out UserRoomConnection userRoomConnection)){
            //     return base.OnDisconnectedAsync(exp);
            // }
            // Clients.Client(Context.ConnectionId).SendAsync("ReceieveMessage","Server",$"{userRoomConnection.UserId} has loggedOut");
            // SendConnectedUser("App");

            _connections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exp);
        }

        // public Task SendConnectedUser(string room)
        // {
        //     var users = _connections.Values.Select(u=>u.UserId);
        //     return  Clients.Group(room).SendAsync("ConnectedUser",users);
        // }
    }
}
