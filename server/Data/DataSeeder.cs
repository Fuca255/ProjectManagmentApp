using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Authorization;
using server.Models;

namespace server.Data
{
    public class DataSeeder
    {
        private readonly ProjectManagmentDbContext _context;

        public DataSeeder(ProjectManagmentDbContext context)
        {
            _context =context;
        }
        List<Permission> permissions = new List<Permission>();
        public void Seed()
        {

            //prioriteti
            if(!_context.Priorities.Any())
            {
                var priorities = new List<Priority>{
                    new Priority{
                        Id = 1,
                        Level = 4,
                        Description = "At risk"
                    },
                    new Priority{
                        Id = 2,
                        Level = 3,
                        Description = "High"
                    },
                    new Priority{
                        Id = 3,
                        Level = 2,
                        Description = "Medium"
                    },
                    new Priority{
                        Id = 4,
                        Level = 1,
                        Description = "Low"
                    },
                };
                _context.Priorities.AddRange(priorities);
                _context.SaveChanges();
            }

            if(!_context.States.Any())
            {
                var states = new List<State>{
                    new State{
                        Id = 1,
                        Name = "Not started"
                    },
                    new State{
                        Id = 2,
                        Name = "Ready"
                    },
                    new State{
                        Id = 3,
                        Name = "In Progress"
                    },
                    new State{
                        Id = 4,
                        Name = "Blocked"
                    },
                    new State{
                        Id = 5,
                        Name = "Done"
                    },
                    new State{
                        Id = 6,
                        Name = "Cancelled"
                    }
                };
                _context.States.AddRange(states);
                _context.SaveChanges();
            }
            if (!_context.Permissions.Any())
            {
                permissions = new List<Permission>
                {
                    new Permission { Id = (int)Permisija.ViewProjectDetails, Name = "View Project Details", Description = "Allows users to view project details" },
                    new Permission { Id = (int)Permisija.EditProjectDetails, Name = "Edit Project Details", Description = "Allows users to edit project details" },
                    new Permission { Id = (int)Permisija.AddTask, Name = "Add Task", Description = "Allows users to add tasks to the project" },
                    new Permission { Id = (int)Permisija.DeleteTask, Name = "Delete Task", Description = "Allows users to delete tasks from the project" },
                    new Permission { Id = (int)Permisija.EditTask, Name = "Edit Task", Description = "Allows users to edit tasks in the project" },
                    new Permission { Id = (int)Permisija.AssignTask, Name = "Assign Task", Description = "Allows users to assign tasks to team members" },
                    new Permission { Id = (int)Permisija.AddComment, Name = "Add Comment", Description = "Allows users to add comments to tasks" },
                    new Permission { Id = (int)Permisija.DeleteComment, Name = "Delete Comment", Description = "Allows users to delete their comments from tasks" },
                    new Permission { Id = (int)Permisija.ViewTeamMembers, Name = "View Team Members", Description = "Allows users to view members of the project team" },
                    new Permission { Id = (int)Permisija.EditTeamMembers, Name = "Edit Team Members", Description = "Allows users to edit members of the project team" },
                    new Permission { Id = (int)Permisija.ChangeProjectState, Name = "Change Project State", Description = "Allows users to change the state of the project" }
                };
                _context.Permissions.AddRange(permissions);
                _context.SaveChanges();
            }
            if (!_context.Roles.Any())
            {
                var platformRoles = new List<Role>{
                    new Role{
                        Id = 1,
                        Name = "Project Manager",
                        Authority = 2
                    },
                    new Role{
                        Id = 2,
                        Name = "Project Worker",
                        Authority = 3
                    },
                    new Role{
                        Id = 3,
                        Name = "Member",
                        Authority = 4
                    },
                    new Role{
                        Id = 4,
                        Name = "Guest",
                        Authority = 5
                    },
                    new Role{
                        Id = 5,
                        Name = "Admin",
                        Authority = 1
                    }

                };
                _context.Roles.AddRange(platformRoles);
                _context.SaveChanges();
            }
            if (!_context.RolePermissions.Any())
            {
                // Define role-permission associations here
                var rolePermissions = new List<RolePermission>
                {
                    // Project Manager with ID 1 has all permissions
                    new RolePermission { RoleId = 1, PermissionId = (int)Permisija.ViewProjectDetails },
                    new RolePermission { RoleId = 1, PermissionId = (int)Permisija.EditProjectDetails },
                    new RolePermission { RoleId = 1, PermissionId = (int)Permisija.AddTask },
                    new RolePermission { RoleId = 1, PermissionId = (int)Permisija.DeleteTask },
                    new RolePermission { RoleId = 1, PermissionId = (int)Permisija.EditTask },
                    new RolePermission { RoleId = 1, PermissionId = (int)Permisija.AssignTask },
                    new RolePermission { RoleId = 1, PermissionId = (int)Permisija.AddComment },
                    new RolePermission { RoleId = 1, PermissionId = (int)Permisija.DeleteComment },
                    new RolePermission { RoleId = 1, PermissionId = (int)Permisija.ViewTeamMembers },
                    new RolePermission { RoleId = 1, PermissionId = (int)Permisija.EditTeamMembers },
                    new RolePermission { RoleId = 1, PermissionId = (int)Permisija.ChangeProjectState },
                    // Developer with ID 2 has permissions for adding and deleting comments
                    new RolePermission { RoleId = 2, PermissionId = (int)Permisija.AddComment },
                    new RolePermission { RoleId = 2, PermissionId = (int)Permisija.DeleteComment },
                    // Add more role-permission associations as needed
                };

                _context.RolePermissions.AddRange(rolePermissions);
                _context.SaveChanges();
            }
        
            if(!_context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Id = 1,
                        Name = "John",
                        Lastname = "Doe",
                        Username = "johndoe",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                        Email = "john.doe@example.com",
                        JobTitle = "Project Manager",
                        Organisation = "Organization A",
                        Department = "Department 1",
                        RoleId = 1 // Project Manager
                    },
                    new User
                    {
                        Id = 2,
                        Name = "Aleksandra",
                        Lastname = "Stanic",
                        Username = "aleksandra_stanic",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("Boki037"),
                        Email = "a.stanic@example.com",
                        JobTitle = "Project Manager",
                        Organisation = "Organization B",
                        Department = "Department 2",
                        RoleId = 1 // Project Manager
                    },
                    new User
                    {
                        Id = 3,
                        Name = "Alex",
                        Lastname = "Johnson",
                        Username = "alexjohnson",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("password789"),
                        Email = "alex.johnson@example.com",
                        JobTitle = "Developer",
                        Organisation = "Organization A",
                        Department = "Department 1",
                        RoleId = 2 // Developer
                    },
                    new User
                    {
                        Id = 4,
                        Name = "Emily",
                        Lastname = "Brown",
                        Username = "emilybrown",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("password101"),
                        Email = "emily.brown@example.com",
                        JobTitle = "Developer",
                        Organisation = "Organization B",
                        Department = "Department 2",
                        RoleId = 2 // Developer
                    },
                    new User
                    {
                        Id = 5,
                        Name = "Michael",
                        Lastname = "Wilson",
                        Username = "michaelwilson",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("password202"),
                        Email = "michael.wilson@example.com",
                        JobTitle = "Developer",
                        Organisation = "Organization A",
                        Department = "Department 1",
                        RoleId = 2 // Developer
                    },
                    new User
                    {
                        Id = 6,
                        Name = "Sarah",
                        Lastname = "Martinez",
                        Username = "sarahmartinez",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("password303"),
                        Email = "sarah.martinez@example.com",
                        JobTitle = "Developer",
                        Organisation = "Organization B",
                        Department = "Department 2",
                        RoleId = 2 // Developer
                    },
                    new User
                    {
                        Id = 7,
                        Name = "Jessica",
                        Lastname = "Lee",
                        Username = "jessicalee",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("password404"),
                        Email = "jessica.lee@example.com",
                        JobTitle = "User",
                        Organisation = "Organization A",
                        Department = "Department 1",
                        RoleId = 3 // User
                    },
                    new User
                    {
                        Id = 8,
                        Name = "William",
                        Lastname = "Taylor",
                        Username = "williamtaylor",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("password505"),
                        Email = "william.taylor@example.com",
                        JobTitle = "User",
                        Organisation = "Organization B",
                        Department = "Department 2",
                        RoleId = 3 // User
                    },
                    new User
                    {
                        Id = 9,
                        Name = "Olivia",
                        Lastname = "Anderson",
                        Username = "oliviaanderson",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("password606"),
                        Email = "olivia.anderson@example.com",
                        JobTitle = "User",
                        Organisation = "Organization A",
                        Department = "Department 1",
                        RoleId = 4 // Guest
                    },
                    new User
                    {
                        Id = 10,
                        Name = "Ethan",
                        Lastname = "Moore",
                        Username = "ethanmoore",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("password707"),
                        Email = "ethan.moore@example.com",
                        JobTitle = "Admin",
                        Organisation = "Organization B",
                        Department = "Department 2",
                        RoleId = 5 // Admin
                    },
                    new User
                    {
                        Id = 11,
                        Name = "Michael",
                        Lastname = "Smith",
                        Username = "michaelsmith",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("mypass123"),
                        Email = "michael.smith@example.com",
                        JobTitle = "Developer",
                        Organisation = "Organization B",
                        Department = "Development",
                        RoleId = 2 // Developer
                    },
                    new User
                    {
                        Id = 12,
                        Name = "Emily",
                        Lastname = "Johnson",
                        Username = "emilyjohnson",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                        Email = "emily.johnson@example.com",
                        JobTitle = "Analyst",
                        Organisation = "Organization C",
                        Department = "Analytics",
                        RoleId = 4 // Guest
                    },
                    new User
                    {
                        Id = 13,
                        Name = "David",
                        Lastname = "Brown",
                        Username = "davidbrown",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("brownie567"),
                        Email = "david.brown@example.com",
                        JobTitle = "Manager",
                        Organisation = "Organization D",
                        Department = "Management",
                        RoleId = 1 // Project Manager
                    },
                    new User
                    {
                        Id = 14,
                        Name = "Sarah",
                        Lastname = "Wilson",
                        Username = "sarahwilson",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("ilovesarah"),
                        Email = "sarah.wilson@example.com",
                        JobTitle = "Consultant",
                        Organisation = "Organization E",
                        Department = "Consulting",
                        RoleId = 5 // Admin
                    },
                    new User
                    {
                        Id = 15,
                        Name = "Lauren",
                        Lastname = "Thomas",
                        Username = "laurenthomas",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("laurenpass"),
                        Email = "lauren.thomas@example.com",
                        JobTitle = "Coordinator",
                        Organisation = "Organization I",
                        Department = "Coordination",
                        RoleId = 4 // Guest
                    },
                    new User
                    {
                        Id = 16,
                        Name = "Daniel",
                        Lastname = "Jackson",
                        Username = "danieljackson",
                        IsActivated = true,
                        Password = BCrypt.Net.BCrypt.HashPassword("daniel567"),
                        Email = "daniel.jackson@example.com",
                        JobTitle = "Specialist",
                        Organisation = "Organization J",
                        Department = "Specialization",
                        RoleId = 3 // User
                    },
                };

                _context.Users.AddRange(users);
                _context.SaveChanges();

                if(!_context.Teams.Any())
                {
                    //Teams
                    var teams = new List<Team>
                    {
                        new Team { Id = 1, Name = "Network", Type = "Type 1", CreatedBy = users[0].Id, Users = new List<User>{users[2],users[3],users[6]} },
                        new Team { Id = 2, Name = "Projektanti", Type = "Type 2", CreatedBy = users[1].Id, Users = new List<User>{users[5],users[4],users[7]} },
                        new Team { Id = 3, Name = "Bekend", Type = "Type 1", CreatedBy = users[0].Id, Users = new List<User>{users[8],users[2],users[4]} },
                        new Team { Id = 4, Name = "Frontend", Type = "Type 2", CreatedBy = users[1].Id, Users = new List<User>{users[4],users[3],users[6]} },
                        new Team { Id = 5, Name = "Workeri", Type = "Type 1", CreatedBy = users[0].Id, Users = new List<User>{users[5],users[7],users[8]} }
                    };

                    _context.Teams.AddRange(teams);
                    _context.SaveChanges();

                    // Assign users to teams TO-DO


                    if(!_context.Projects.Any())
                    {
                        //Projects
                        var projects = new List<Project>
                        {
                            new Project
                            {
                                Id = 1,
                                Title = "Website Redesign",
                                StateId = 1,
                                Percentage = 100,
                                Start = new DateTime(2023, 1, 15),
                                End = new DateTime(2023, 5, 30),
                                Description = "Redesign the company's website with a modern and responsive design.",
                                LastStateChangedTime = DateTime.Now,
                                Budget = 10000,
                                Spent = 8000,
                                Owner = users[0],
                                Type = "Web",
                                PriorityId = 2, // Medium Priority
                                ProjectUsers = new List<ProjectUser>{
                                    new ProjectUser{
                                        ProjectId = 1,
                                        User = users[2],
                                        Role = users[2].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 1,
                                        User = users[3],
                                        Role = users[3].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 1,
                                        User = users[6],
                                        Role = users[6].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 1,
                                        User = users[0],
                                        Role = users[0].Role
                                    },
                                }
                            },
                            new Project
                            {
                                Id = 2,
                                Title = "Marketing Campaign",
                                StateId = 2,
                                Percentage = 80,
                                Start = new DateTime(2023, 2, 10),
                                End = new DateTime(2023, 6, 30),
                                Description = "Plan and execute a targeted marketing campaign for new product launch.",
                                LastStateChangedTime = DateTime.Now,
                                Budget = 12000,
                                Spent = 10000,
                                Owner = users[1],
                                Type = "Marketing",
                                PriorityId = 1, // High Priority
                                ProjectUsers = new List<ProjectUser>{
                                    new ProjectUser{
                                        ProjectId = 2,
                                        User = users[1],
                                        Role = users[1].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 2,
                                        User = users[5],
                                        Role = users[5].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 2,
                                        User = users[4],
                                        Role = users[4].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 2,
                                        User = users[7],
                                        Role = users[7].Role
                                    },
                                }
                            },
                            new Project
                            {
                                Id = 3,
                                Title = "Data Analysis Tool",
                                StateId = 3,
                                Percentage = 60,
                                Start = new DateTime(2023, 3, 5),
                                End = DateTime.MinValue, // Project not ended yet
                                Description = "Develop a tool for analyzing large datasets and generating reports.",
                                LastStateChangedTime = DateTime.Now,
                                Budget = 15000,
                                Spent = 12000,
                                Owner = users[0],
                                Type = "Data",
                                PriorityId = 3, // Low Priority
                                ProjectUsers = new List<ProjectUser>{
                                    new ProjectUser{
                                        ProjectId = 3,
                                        User = users[8],
                                        Role = users[8].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 3,
                                        User = users[0],
                                        Role = users[0].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 3,
                                        User = users[2],
                                        Role = users[2].Role
                                    },
                                    new ProjectUser{
                                        ProjectId =3,
                                        User = users[4],
                                        Role = users[4].Role
                                    },
                                }
                            },
                            new Project
                            {
                                Id = 4,
                                Title = "Product Enhancement",
                                StateId = 1,
                                Percentage = 100,
                                Start = new DateTime(2023, 4, 20),
                                End = new DateTime(2023, 8, 31),
                                Description = "Enhance product features based on user feedback and market trends.",
                                LastStateChangedTime = DateTime.Now,
                                Budget = 18000,
                                Spent = 15000,
                                Owner = users[1],
                                Type = "Product",
                                PriorityId = 2, // Medium Priority
                                ProjectUsers = new List<ProjectUser>{
                                    new ProjectUser{
                                        ProjectId = 4,
                                        User = users[1],
                                        Role = users[1].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 4,
                                        User = users[3],
                                        Role = users[3].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 4,
                                        User = users[4],
                                        Role = users[4].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 4,
                                        User = users[6],
                                        Role = users[6].Role
                                    },
                                }
                            },
                            new Project
                            {
                                Id = 5,
                                Title = "Training Platform",
                                StateId = 2,
                                Percentage = 40,
                                Start = new DateTime(2023, 5, 15),
                                End = DateTime.MinValue, // Project not ended yet
                                Description = "Develop an online training platform for employees.",
                                LastStateChangedTime = DateTime.Now,
                                Budget = 8000,
                                Spent = 6000,
                                Owner = users[0],
                                Type = "Training",
                                PriorityId = 1, // High Priority
                                ProjectUsers = new List<ProjectUser>{
                                    new ProjectUser{
                                        ProjectId = 5,
                                        User = users[5],
                                        Role = users[5].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 5,
                                        User = users[7],
                                        Role = users[7].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 5,
                                        User = users[8],
                                        Role = users[8].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 5,
                                        User = users[0],
                                        Role = users[0].Role
                                    },
                                }
                            },
                            new Project
                            {
                                Id = 6,
                                Title = "Mobile App Development",
                                StateId = 2,
                                Percentage = 60,
                                Start = new DateTime(2023, 6, 15),
                                End = DateTime.MinValue, // Project not ended yet
                                Description = "Develop a mobile application for iOS and Android platforms.",
                                LastStateChangedTime = DateTime.Now,
                                Budget = 8000,
                                Spent = 6000,
                                Owner = users[0],
                                Type = "Mobile",
                                PriorityId = 3, // Low Priority
                                ProjectUsers = new List<ProjectUser>{
                                    new ProjectUser{
                                        ProjectId = 6,
                                        User = users[2],
                                        Role = users[2].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 6,
                                        User = users[3],
                                        Role = users[3].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 6,
                                        User = users[6],
                                        Role = users[6].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 6,
                                        User = users[0],
                                        Role = users[0].Role
                                    },
                                }
                            },
                            new Project
                            {
                                Id = 7,
                                Title = "Software Upgrade",
                                StateId = 1,
                                Percentage = 100,
                                Start = new DateTime(2023, 7, 1),
                                End = new DateTime(2023, 9, 30),
                                Description = "Upgrade existing software to the latest version with new features.",
                                LastStateChangedTime = DateTime.Now,
                                Budget = 10000,
                                Spent = 9000,
                                Owner = users[1],
                                Type = "Software",
                                PriorityId = 2, // Medium Priority
                                ProjectUsers = new List<ProjectUser>{
                                    new ProjectUser{
                                        ProjectId = 7,
                                        User = users[1],
                                        Role = users[1].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 7,
                                        User = users[5],
                                        Role = users[5].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 7,
                                        User = users[4],
                                        Role = users[4].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 7,
                                        User = users[7],
                                        Role = users[7].Role
                                    },
                                }
                            },
                            new Project
                            {
                                Id = 8,
                                Title = "Database Optimization",
                                StateId = 2,
                                Percentage = 40,
                                Start = new DateTime(2023, 8, 10),
                                End = new DateTime(2023, 11, 30),
                                Description = "Optimize database performance for faster data retrieval.",
                                LastStateChangedTime = DateTime.Now,
                                Budget = 5000,
                                Spent = 3500,
                                Owner = users[0],
                                Type = "Database",
                                PriorityId = 1, // High Priority
                                ProjectUsers = new List<ProjectUser>{
                                    new ProjectUser{
                                        ProjectId = 8,
                                        User = users[8],
                                        Role = users[8].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 8,
                                        User = users[0],
                                        Role = users[0].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 8,
                                        User = users[2],
                                        Role = users[2].Role
                                    },
                                    new ProjectUser{
                                        ProjectId =8,
                                        User = users[4],
                                        Role = users[4].Role
                                    },
                                }
                            },
                            new Project
                            {
                                Id = 9,
                                Title = "Network Security",
                                StateId = 3,
                                Percentage = 80,
                                Start = new DateTime(2023, 9, 5),
                                End = new DateTime(2023, 12, 15),
                                Description = "Implement enhanced security measures for the company's network infrastructure.",
                                LastStateChangedTime = DateTime.Now,
                                Budget = 12000,
                                Spent = 10000,
                                Owner = users[1],
                                Type = "Security",
                                PriorityId = 2, // Medium Priority
                                ProjectUsers = new List<ProjectUser>{
                                    new ProjectUser{
                                        ProjectId = 9,
                                        User = users[1],
                                        Role = users[1].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 9,
                                        User = users[3],
                                        Role = users[3].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 9,
                                        User = users[4],
                                        Role = users[4].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 9,
                                        User = users[6],
                                        Role = users[6].Role
                                    },
                                }
                            },
                            new Project
                            {
                                Id = 10,
                                Title = "Customer Support System",
                                StateId = 4,
                                Percentage = 30,
                                Start = new DateTime(2023, 10, 20),
                                End = new DateTime(2024, 3, 31),
                                Description = "Develop a customer support system to streamline customer inquiries.",
                                LastStateChangedTime = DateTime.Now,
                                Budget = 15000,
                                Spent = 8000,
                                Owner = users[0],
                                Type = "Support",
                                PriorityId = 3, // Low Priority
                                ProjectUsers = new List<ProjectUser>{
                                    new ProjectUser{
                                        ProjectId = 10,
                                        User = users[5],
                                        Role = users[5].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 10,
                                        User = users[7],
                                        Role = users[7].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 10,
                                        User = users[8],
                                        Role = users[8].Role
                                    },
                                    new ProjectUser{
                                        ProjectId = 10,
                                        User = users[0],
                                        Role = users[0].Role
                                    },
                                }
                            },

                        };
                        //TO-DO uraditi initial task-grupe za projekte
                        for(int i = 1;i<=10;i++)
                        {
                            var group = new TaskGroup{
                                Id = i,
                                ProjectId = i,
                                Title = projects[i-1].Title,
                                ParentTaskGroupId = null
                            };
                            _context.TaskGroups.Add(group);
                            var subGroup = new TaskGroup
                            {
                                Title = "Podgrupa",
                                ProjectId = i,
                                ParentTaskGroupId = group.Id
                            };
                            _context.TaskGroups.Add(subGroup);
                        }
                        _context.Projects.AddRange(projects);
                        _context.SaveChanges();

                        if(!_context.Assignments.Any())
                        {
                            var assignments = new List<Assignment>{
                                //project 1 rok (2023, 1, 15) do (2023, 5, 30),members:1,3,4,7
                                new Assignment{
                                    Id = 1,
                                    Title = "Develop Software Features",
                                    Owner = 1,
                                    Description = "Implement new functionalities in the company`s software products",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 6, 14),
                                    StateId = 2,
                                    Percentage = 35,
                                    PriorityId = 3,
                                    Type = "Tip 1",
                                    TaskGroupId = 2,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4]},
                                    
                                },
                                new Assignment{
                                    Id = 2,
                                    Title = "Conduct Market Research",
                                    Owner = 1,
                                    Description = "Gather and analyze data about market trends and customer preferences",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 5, 16),
                                    StateId = 3,
                                    Percentage = 30,
                                    PriorityId = 2,
                                    Type = "Tip 2",
                                    TaskGroupId = 2,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[5],users[7]},
                                    
                                },
                                new Assignment{
                                    Id = 3,
                                    Title = "Manage Social Media",
                                    Owner = 1,
                                    Description = "Create and schedule posts",
                                    Start = new DateTime(2023, 1, 25),
                                    End = new DateTime(2023, 2, 14),
                                    StateId = 4,
                                    Percentage = 50,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 2,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4]},
                                    
                                },
                                //project 2 rok (2023, 2, 10) do (2023, 6, 30), members:2,5,6,8
                                new Assignment{
                                    Id = 4,
                                    Title = "Perform Data Analysis",
                                    Owner = 2,
                                    Description = "Analyze company data to provide insights",
                                    Start = new DateTime(2023, 3, 1),
                                    End = new DateTime(2023, 3, 15),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 1",
                                    TaskGroupId = 2,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[5],users[7]},
                                    
                                },
                                new Assignment{
                                    Id = 5,
                                    Title = "Customer Support",
                                    Owner = 2,
                                    Description = "Assist customers with inquiries",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 2,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[5],users[7]},
                                    
                                },
                                new Assignment{
                                    Id = 6,
                                    Title = "Organize events",
                                    Owner = 2,
                                    Description = "Plan and execute company events",
                                    Start = new DateTime(2023, 3, 3),
                                    End = new DateTime(2023, 3, 20),
                                    StateId = 2,
                                    Percentage = 45,
                                    PriorityId = 4,
                                    Type = "Tip 2",
                                    TaskGroupId = 2,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1]},
                                    
                                },
                                //project 3 rok (2024, 3, 5) - jos traje,members:9,1,3,5
                                new Assignment{
                                    Id = 7,
                                    Title = "Maintain IT Systems",
                                    Owner = 1,
                                    Description = "Ensure the company`s IT infrastructure is secure",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 44,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 2,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[7],users[1]},
                                    
                                },
                                new Assignment{
                                    Id = 8,
                                    Title = "Prepare Financial Reports",
                                    Owner = 1,
                                    Description = "Compile and present financial data to assist with budgeting",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023,5, 14),
                                    StateId = 2,
                                    Percentage = 66,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 2,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4]},
                                    
                                },
                                new Assignment{
                                    Id = 9,
                                    Title = "Create Marketing Campaign",
                                    Owner = 1,
                                    Description = "Design and launch marketing initiatives to promote products and services",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 3, 25),
                                    StateId = 2,
                                    Percentage = 10,
                                    PriorityId = 1,
                                    Type = "Tip 1",
                                    TaskGroupId = 2,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[5]},
                                    
                                },
                                    //project 4 rok (2023, 4, 20) do (2023, 8, 31),members:2,4,5,7
                                new Assignment{
                                    Id = 10,
                                    Title = "Conduct Employee Training",
                                    Owner = 2,
                                    Description = "Develop and deliver training programs to enhance employee skils and knowledge",
                                    Start = new DateTime(2023, 4, 21),
                                    End = new DateTime(2023, 4, 30),
                                    StateId = 3,
                                    Percentage = 50,
                                    PriorityId = 1,
                                    Type = "Tip 2",
                                    TaskGroupId = 2,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4],users[7]},
                                    
                                },
                                new Assignment{
                                    Id = 11,
                                    Title = "Develop Software Features",
                                    Owner = 1,
                                    Description = "Implement new functionalities in the company`s software products",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 6, 14),
                                    StateId = 2,
                                    Percentage = 35,
                                    PriorityId = 3,
                                    Type = "Tip 1",
                                    TaskGroupId = 4,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[4],users[6]},

                                },
                                new Assignment{
                                    Id = 12,
                                    Title = "Conduct Market Research",
                                    Owner = 1,
                                    Description = "Gather and analyze data about market trends and customer preferences",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 5, 16),
                                    StateId = 3,
                                    Percentage = 30,
                                    PriorityId = 2,
                                    Type = "Tip 2",
                                    TaskGroupId = 4,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[3],users[6]},

                                },
                                new Assignment{
                                    Id = 13,
                                    Title = "Manage Social Media",
                                    Owner = 1,
                                    Description = "Create and schedule posts",
                                    Start = new DateTime(2023, 1, 25),
                                    End = new DateTime(2023, 2, 14),
                                    StateId = 4,
                                    Percentage = 50,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 4,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[3]},

                                },
                                //project 2 rok (2023, 2, 10) do (2023, 6, 30), members:2,5,6,8
                                new Assignment{
                                    Id = 14,
                                    Title = "Perform Data Analysis",
                                    Owner = 2,
                                    Description = "Analyze company data to provide insights",
                                    Start = new DateTime(2023, 3, 1),
                                    End = new DateTime(2023, 3, 15),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 1",
                                    TaskGroupId = 4,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4]},

                                },
                                new Assignment{
                                    Id = 15,
                                    Title = "Customer Support",
                                    Owner = 2,
                                    Description = "Assist customers with inquiries",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 4,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[6],users[4]},

                                },
                                new Assignment{
                                    Id = 16,
                                    Title = "Organize events",
                                    Owner = 2,
                                    Description = "Plan and execute company events",
                                    Start = new DateTime(2023, 3, 3),
                                    End = new DateTime(2023, 3, 20),
                                    StateId = 2,
                                    Percentage = 45,
                                    PriorityId = 4,
                                    Type = "Tip 2",
                                    TaskGroupId = 4,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1]},

                                },
                                //project 3 rok (2024, 3, 5) - jos traje,members:9,1,3,5
                                new Assignment{
                                    Id = 17,
                                    Title = "Maintain IT Systems",
                                    Owner = 1,
                                    Description = "Ensure the company`s IT infrastructure is secure",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 44,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 4,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[6],users[4]},

                                },
                                new Assignment{
                                    Id = 18,
                                    Title = "Prepare Financial Reports",
                                    Owner = 1,
                                    Description = "Compile and present financial data to assist with budgeting",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023,5, 14),
                                    StateId = 2,
                                    Percentage = 66,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 4,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4]},

                                },
                                new Assignment{
                                    Id = 19,
                                    Title = "Create Marketing Campaign",
                                    Owner = 1,
                                    Description = "Design and launch marketing initiatives to promote products and services",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 3, 25),
                                    StateId = 2,
                                    Percentage = 10,
                                    PriorityId = 1,
                                    Type = "Tip 1",
                                    TaskGroupId = 4,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4]},

                                },
                                    //project 4 rok (2023, 4, 20) do (2023, 8, 31),members:2,4,5,7
                                new Assignment{
                                    Id = 20,
                                    Title = "Conduct Employee Training",
                                    Owner = 2,
                                    Description = "Develop and deliver training programs to enhance employee skils and knowledge",
                                    Start = new DateTime(2023, 4, 21),
                                    End = new DateTime(2023, 4, 30),
                                    StateId = 3,
                                    Percentage = 50,
                                    PriorityId = 1,
                                    Type = "Tip 2",
                                    TaskGroupId = 4,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4],users[6]},

                                },
                                new Assignment{
                                    Id = 21,
                                    Title = "Develop Software Features",
                                    Owner = 1,
                                    Description = "Implement new functionalities in the company`s software products",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 6, 14),
                                    StateId = 2,
                                    Percentage = 35,
                                    PriorityId = 3,
                                    Type = "Tip 1",
                                    TaskGroupId = 7,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4]},

                                },
                                new Assignment{
                                    Id = 22,
                                    Title = "Conduct Market Research",
                                    Owner = 1,
                                    Description = "Gather and analyze data about market trends and customer preferences",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 5, 16),
                                    StateId = 3,
                                    Percentage = 30,
                                    PriorityId = 2,
                                    Type = "Tip 2",
                                    TaskGroupId = 7,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[5],users[7]},

                                },
                                new Assignment{
                                    Id = 23,
                                    Title = "Manage Social Media",
                                    Owner = 1,
                                    Description = "Create and schedule posts",
                                    Start = new DateTime(2023, 1, 25),
                                    End = new DateTime(2023, 2, 14),
                                    StateId = 4,
                                    Percentage = 50,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 7,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4]},

                                },
                                //project 2 rok (2023, 2, 10) do (2023, 6, 30), members:2,5,6,8
                                new Assignment{
                                    Id = 24,
                                    Title = "Perform Data Analysis",
                                    Owner = 2,
                                    Description = "Analyze company data to provide insights",
                                    Start = new DateTime(2023, 3, 1),
                                    End = new DateTime(2023, 3, 15),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 1",
                                    TaskGroupId = 7,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[5],users[7]},

                                },
                                new Assignment{
                                    Id = 25,
                                    Title = "Customer Support",
                                    Owner = 2,
                                    Description = "Assist customers with inquiries",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 7,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[5],users[7]},

                                },
                                new Assignment{
                                    Id = 26,
                                    Title = "Organize events",
                                    Owner = 2,
                                    Description = "Plan and execute company events",
                                    Start = new DateTime(2023, 3, 3),
                                    End = new DateTime(2023, 3, 20),
                                    StateId = 2,
                                    Percentage = 45,
                                    PriorityId = 4,
                                    Type = "Tip 2",
                                    TaskGroupId = 7,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1]},

                                },
                                //project 3 rok (2024, 3, 5) - jos traje,members:9,1,3,5
                                new Assignment{
                                    Id = 27,
                                    Title = "Maintain IT Systems",
                                    Owner = 1,
                                    Description = "Ensure the company`s IT infrastructure is secure",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 44,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 7,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[7],users[4]},

                                },
                                new Assignment{
                                    Id = 28,
                                    Title = "Prepare Financial Reports",
                                    Owner = 1,
                                    Description = "Compile and present financial data to assist with budgeting",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023,5, 14),
                                    StateId = 2,
                                    Percentage = 66,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 7,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[5]},

                                },
                                new Assignment{
                                    Id = 29,
                                    Title = "Create Marketing Campaign",
                                    Owner = 1,
                                    Description = "Design and launch marketing initiatives to promote products and services",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 3, 25),
                                    StateId = 2,
                                    Percentage = 10,
                                    PriorityId = 1,
                                    Type = "Tip 1",
                                    TaskGroupId = 7,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[4],users[5]},

                                },
                                    //project 4 rok (2023, 4, 20) do (2023, 8, 31),members:2,4,5,7
                                new Assignment{
                                    Id = 30,
                                    Title = "Conduct Employee Training",
                                    Owner = 2,
                                    Description = "Develop and deliver training programs to enhance employee skils and knowledge",
                                    Start = new DateTime(2023, 4, 21),
                                    End = new DateTime(2023, 4, 30),
                                    StateId = 3,
                                    Percentage = 50,
                                    PriorityId = 1,
                                    Type = "Tip 2",
                                    TaskGroupId = 7,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4],users[7]},

                                },
                                new Assignment{
                                    Id = 31,
                                    Title = "Develop Software Features",
                                    Owner = 1,
                                    Description = "Implement new functionalities in the company`s software products",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 6, 14),
                                    StateId = 2,
                                    Percentage = 35,
                                    PriorityId = 3,
                                    Type = "Tip 1",
                                    TaskGroupId = 9,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[3],users[4]},

                                },
                                new Assignment{
                                    Id = 32,
                                    Title = "Conduct Market Research",
                                    Owner = 1,
                                    Description = "Gather and analyze data about market trends and customer preferences",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 5, 16),
                                    StateId = 3,
                                    Percentage = 30,
                                    PriorityId = 2,
                                    Type = "Tip 2",
                                    TaskGroupId = 9,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[4],users[3]},

                                },
                                new Assignment{
                                    Id = 33,
                                    Title = "Manage Social Media",
                                    Owner = 1,
                                    Description = "Create and schedule posts",
                                    Start = new DateTime(2023, 1, 25),
                                    End = new DateTime(2023, 2, 14),
                                    StateId = 4,
                                    Percentage = 50,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 9,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[3],users[4]},

                                },
                                //project 2 rok (2023, 2, 10) do (2023, 6, 30), members:2,5,6,8
                                new Assignment{
                                    Id = 34,
                                    Title = "Perform Data Analysis",
                                    Owner = 2,
                                    Description = "Analyze company data to provide insights",
                                    Start = new DateTime(2023, 3, 1),
                                    End = new DateTime(2023, 3, 15),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 1",
                                    TaskGroupId = 9,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[3],users[4]},

                                },
                                new Assignment{
                                    Id = 35,
                                    Title = "Customer Support",
                                    Owner = 2,
                                    Description = "Assist customers with inquiries",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 9,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[6],users[3]},

                                },
                                new Assignment{
                                    Id = 36,
                                    Title = "Organize events",
                                    Owner = 2,
                                    Description = "Plan and execute company events",
                                    Start = new DateTime(2023, 3, 3),
                                    End = new DateTime(2023, 3, 20),
                                    StateId = 2,
                                    Percentage = 45,
                                    PriorityId = 4,
                                    Type = "Tip 2",
                                    TaskGroupId = 9,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1]},

                                },
                                //project 3 rok (2024, 3, 5) - jos traje,members:9,1,3,5
                                new Assignment{
                                    Id = 37,
                                    Title = "Maintain IT Systems",
                                    Owner = 1,
                                    Description = "Ensure the company`s IT infrastructure is secure",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 44,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 9,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[6],users[4]},

                                },
                                new Assignment{
                                    Id = 38,
                                    Title = "Prepare Financial Reports",
                                    Owner = 1,
                                    Description = "Compile and present financial data to assist with budgeting",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023,5, 14),
                                    StateId = 2,
                                    Percentage = 66,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 9,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[3],users[4]},

                                },
                                new Assignment{
                                    Id = 39,
                                    Title = "Create Marketing Campaign",
                                    Owner = 1,
                                    Description = "Design and launch marketing initiatives to promote products and services",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 3, 25),
                                    StateId = 2,
                                    Percentage = 10,
                                    PriorityId = 1,
                                    Type = "Tip 1",
                                    TaskGroupId = 9,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[3],users[4]},

                                },
                                    //project 4 rok (2023, 4, 20) do (2023, 8, 31),members:2,4,5,7
                                new Assignment{
                                    Id = 40,
                                    Title = "Conduct Employee Training",
                                    Owner = 2,
                                    Description = "Develop and deliver training programs to enhance employee skils and knowledge",
                                    Start = new DateTime(2023, 4, 21),
                                    End = new DateTime(2023, 4, 30),
                                    StateId = 3,
                                    Percentage = 50,
                                    PriorityId = 1,
                                    Type = "Tip 2",
                                    TaskGroupId = 9,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[1],users[4],users[6]},

                                },
                                new Assignment{
                                    Id = 41,
                                    Title = "Develop Software Features",
                                    Owner = 1,
                                    Description = "Implement new functionalities in the company`s software products",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 6, 14),
                                    StateId = 2,
                                    Percentage = 35,
                                    PriorityId = 3,
                                    Type = "Tip 1",
                                    TaskGroupId = 1,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[3]},

                                },
                                new Assignment{
                                    Id = 42,
                                    Title = "Conduct Market Research",
                                    Owner = 1,
                                    Description = "Gather and analyze data about market trends and customer preferences",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 5, 16),
                                    StateId = 3,
                                    Percentage = 30,
                                    PriorityId = 2,
                                    Type = "Tip 2",
                                    TaskGroupId = 1,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[3]},

                                },
                                new Assignment{
                                    Id = 43,
                                    Title = "Manage Social Media",
                                    Owner = 1,
                                    Description = "Create and schedule posts",
                                    Start = new DateTime(2023, 1, 25),
                                    End = new DateTime(2023, 2, 14),
                                    StateId = 4,
                                    Percentage = 50,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 1,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[3]},

                                },
                                //project 2 rok (2023, 2, 10) do (2023, 6, 30), members:2,5,6,8
                                new Assignment{
                                    Id = 44,
                                    Title = "Perform Data Analysis",
                                    Owner = 2,
                                    Description = "Analyze company data to provide insights",
                                    Start = new DateTime(2023, 3, 1),
                                    End = new DateTime(2023, 3, 15),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 1",
                                    TaskGroupId = 1,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[6]},

                                },
                                new Assignment{
                                    Id = 45,
                                    Title = "Customer Support",
                                    Owner = 2,
                                    Description = "Assist customers with inquiries",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 1,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[3]},

                                },
                                new Assignment{
                                    Id = 46,
                                    Title = "Organize events",
                                    Owner = 2,
                                    Description = "Plan and execute company events",
                                    Start = new DateTime(2023, 3, 3),
                                    End = new DateTime(2023, 3, 20),
                                    StateId = 2,
                                    Percentage = 45,
                                    PriorityId = 4,
                                    Type = "Tip 2",
                                    TaskGroupId = 1,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2]},

                                },
                                //project 3 rok (2024, 3, 5) - jos traje,members:9,1,3,5
                                new Assignment{
                                    Id = 47,
                                    Title = "Maintain IT Systems",
                                    Owner = 1,
                                    Description = "Ensure the company`s IT infrastructure is secure",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 44,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 1,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2],users[0]},

                                },
                                new Assignment{
                                    Id = 48,
                                    Title = "Prepare Financial Reports",
                                    Owner = 1,
                                    Description = "Compile and present financial data to assist with budgeting",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023,5, 14),
                                    StateId = 2,
                                    Percentage = 66,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 1,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2],users[0]},

                                },
                                new Assignment{
                                    Id = 49,
                                    Title = "Create Marketing Campaign",
                                    Owner = 1,
                                    Description = "Design and launch marketing initiatives to promote products and services",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 3, 25),
                                    StateId = 2,
                                    Percentage = 10,
                                    PriorityId = 1,
                                    Type = "Tip 1",
                                    TaskGroupId = 1,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[6]},

                                },
                                    //project 4 rok (2023, 4, 20) do (2023, 8, 31),members:2,4,5,7
                                new Assignment{
                                    Id = 50,
                                    Title = "Conduct Employee Training",
                                    Owner = 2,
                                    Description = "Develop and deliver training programs to enhance employee skils and knowledge",
                                    Start = new DateTime(2023, 4, 21),
                                    End = new DateTime(2023, 4, 30),
                                    StateId = 3,
                                    Percentage = 50,
                                    PriorityId = 1,
                                    Type = "Tip 2",
                                    TaskGroupId = 1,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2],users[3],users[0]},

                                },
                                new Assignment{
                                    Id = 51,
                                    Title = "Develop Software Features",
                                    Owner = 1,
                                    Description = "Implement new functionalities in the company`s software products",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 6, 14),
                                    StateId = 2,
                                    Percentage = 35,
                                    PriorityId = 3,
                                    Type = "Tip 1",
                                    TaskGroupId = 3,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[2]},

                                },
                                new Assignment{
                                    Id = 52,
                                    Title = "Conduct Market Research",
                                    Owner = 1,
                                    Description = "Gather and analyze data about market trends and customer preferences",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 5, 16),
                                    StateId = 3,
                                    Percentage = 30,
                                    PriorityId = 2,
                                    Type = "Tip 2",
                                    TaskGroupId = 3,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[4],users[8]},

                                },
                                new Assignment{
                                    Id = 53,
                                    Title = "Manage Social Media",
                                    Owner = 1,
                                    Description = "Create and schedule posts",
                                    Start = new DateTime(2023, 1, 25),
                                    End = new DateTime(2023, 2, 14),
                                    StateId = 4,
                                    Percentage = 50,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 3,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[2]},

                                },
                                //project 2 rok (2023, 2, 10) do (2023, 6, 30), members:2,5,6,8
                                new Assignment{
                                    Id = 54,
                                    Title = "Perform Data Analysis",
                                    Owner = 2,
                                    Description = "Analyze company data to provide insights",
                                    Start = new DateTime(2023, 3, 1),
                                    End = new DateTime(2023, 3, 15),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 1",
                                    TaskGroupId = 3,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[8],users[4]},

                                },
                                new Assignment{
                                    Id = 55,
                                    Title = "Customer Support",
                                    Owner = 2,
                                    Description = "Assist customers with inquiries",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 3,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[4]},

                                },
                                new Assignment{
                                    Id = 56,
                                    Title = "Organize events",
                                    Owner = 2,
                                    Description = "Plan and execute company events",
                                    Start = new DateTime(2023, 3, 3),
                                    End = new DateTime(2023, 3, 20),
                                    StateId = 2,
                                    Percentage = 45,
                                    PriorityId = 4,
                                    Type = "Tip 2",
                                    TaskGroupId = 3,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2]},

                                },
                                //project 3 rok (2024, 3, 5) - jos traje,members:9,1,3,5
                                new Assignment{
                                    Id = 57,
                                    Title = "Maintain IT Systems",
                                    Owner = 1,
                                    Description = "Ensure the company`s IT infrastructure is secure",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 44,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 3,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[8],users[0]},

                                },
                                new Assignment{
                                    Id = 58,
                                    Title = "Prepare Financial Reports",
                                    Owner = 1,
                                    Description = "Compile and present financial data to assist with budgeting",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023,5, 14),
                                    StateId = 2,
                                    Percentage = 66,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 3,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[4]},

                                },
                                new Assignment{
                                    Id = 59,
                                    Title = "Create Marketing Campaign",
                                    Owner = 1,
                                    Description = "Design and launch marketing initiatives to promote products and services",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 3, 25),
                                    StateId = 2,
                                    Percentage = 10,
                                    PriorityId = 1,
                                    Type = "Tip 1",
                                    TaskGroupId = 3,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[4]},

                                },
                                    //project 4 rok (2023, 4, 20) do (2023, 8, 31),members:2,4,5,7
                                new Assignment{
                                    Id = 60,
                                    Title = "Conduct Employee Training",
                                    Owner = 2,
                                    Description = "Develop and deliver training programs to enhance employee skils and knowledge",
                                    Start = new DateTime(2023, 4, 21),
                                    End = new DateTime(2023, 4, 30),
                                    StateId = 3,
                                    Percentage = 50,
                                    PriorityId = 1,
                                    Type = "Tip 2",
                                    TaskGroupId = 3,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[4],users[8]},

                                },
                                new Assignment{
                                    Id = 61,
                                    Title = "Develop Software Features",
                                    Owner = 1,
                                    Description = "Implement new functionalities in the company`s software products",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 6, 14),
                                    StateId = 2,
                                    Percentage = 35,
                                    PriorityId = 3,
                                    Type = "Tip 1",
                                    TaskGroupId = 6,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[2]},

                                },
                                new Assignment{
                                    Id = 62,
                                    Title = "Conduct Market Research",
                                    Owner = 1,
                                    Description = "Gather and analyze data about market trends and customer preferences",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 5, 16),
                                    StateId = 3,
                                    Percentage = 30,
                                    PriorityId = 2,
                                    Type = "Tip 2",
                                    TaskGroupId = 6,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[6],users[3]},

                                },
                                new Assignment{
                                    Id = 63,
                                    Title = "Manage Social Media",
                                    Owner = 1,
                                    Description = "Create and schedule posts",
                                    Start = new DateTime(2023, 1, 25),
                                    End = new DateTime(2023, 2, 14),
                                    StateId = 4,
                                    Percentage = 50,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 6,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2],users[3]},

                                },
                                //project 2 rok (2023, 2, 10) do (2023, 6, 30), members:2,5,6,8
                                new Assignment{
                                    Id = 64,
                                    Title = "Perform Data Analysis",
                                    Owner = 2,
                                    Description = "Analyze company data to provide insights",
                                    Start = new DateTime(2023, 3, 1),
                                    End = new DateTime(2023, 3, 15),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 1",
                                    TaskGroupId = 6,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[3]},

                                },
                                new Assignment{
                                    Id = 65,
                                    Title = "Customer Support",
                                    Owner = 2,
                                    Description = "Assist customers with inquiries",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 6,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[3],users[6]},

                                },
                                new Assignment{
                                    Id = 66,
                                    Title = "Organize events",
                                    Owner = 2,
                                    Description = "Plan and execute company events",
                                    Start = new DateTime(2023, 3, 3),
                                    End = new DateTime(2023, 3, 20),
                                    StateId = 2,
                                    Percentage = 45,
                                    PriorityId = 4,
                                    Type = "Tip 2",
                                    TaskGroupId = 6,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0]},

                                },
                                //project 3 rok (2024, 3, 5) - jos traje,members:9,1,3,5
                                new Assignment{
                                    Id = 67,
                                    Title = "Maintain IT Systems",
                                    Owner = 1,
                                    Description = "Ensure the company`s IT infrastructure is secure",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 44,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 6,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[6],users[0]},

                                },
                                new Assignment{
                                    Id = 68,
                                    Title = "Prepare Financial Reports",
                                    Owner = 1,
                                    Description = "Compile and present financial data to assist with budgeting",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023,5, 14),
                                    StateId = 2,
                                    Percentage = 66,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 6,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2],users[3]},

                                },
                                new Assignment{
                                    Id = 69,
                                    Title = "Create Marketing Campaign",
                                    Owner = 1,
                                    Description = "Design and launch marketing initiatives to promote products and services",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 3, 25),
                                    StateId = 2,
                                    Percentage = 10,
                                    PriorityId = 1,
                                    Type = "Tip 1",
                                    TaskGroupId = 6,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[3]},

                                },
                                    //project 4 rok (2023, 4, 20) do (2023, 8, 31),members:2,4,5,7
                                new Assignment{
                                    Id = 70,
                                    Title = "Conduct Employee Training",
                                    Owner = 2,
                                    Description = "Develop and deliver training programs to enhance employee skils and knowledge",
                                    Start = new DateTime(2023, 4, 21),
                                    End = new DateTime(2023, 4, 30),
                                    StateId = 3,
                                    Percentage = 50,
                                    PriorityId = 1,
                                    Type = "Tip 2",
                                    TaskGroupId = 6,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[3],users[6]},

                                },
                                new Assignment{
                                    Id = 71,
                                    Title = "Develop Software Features",
                                    Owner = 1,
                                    Description = "Implement new functionalities in the company`s software products",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 6, 14),
                                    StateId = 2,
                                    Percentage = 35,
                                    PriorityId = 3,
                                    Type = "Tip 1",
                                    TaskGroupId = 8,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[2]},

                                },
                                new Assignment{
                                    Id = 72,
                                    Title = "Conduct Market Research",
                                    Owner = 1,
                                    Description = "Gather and analyze data about market trends and customer preferences",
                                    Start = new DateTime(2023, 3, 25),
                                    End = new DateTime(2023, 5, 16),
                                    StateId = 3,
                                    Percentage = 30,
                                    PriorityId = 2,
                                    Type = "Tip 2",
                                    TaskGroupId = 8,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[4],users[8]},

                                },
                                new Assignment{
                                    Id = 73,
                                    Title = "Manage Social Media",
                                    Owner = 1,
                                    Description = "Create and schedule posts",
                                    Start = new DateTime(2023, 1, 25),
                                    End = new DateTime(2023, 2, 14),
                                    StateId = 4,
                                    Percentage = 50,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 8,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2],users[4]},

                                },
                                //project 2 rok (2023, 2, 10) do (2023, 6, 30), members:2,5,6,8
                                new Assignment{
                                    Id = 74,
                                    Title = "Perform Data Analysis",
                                    Owner = 2,
                                    Description = "Analyze company data to provide insights",
                                    Start = new DateTime(2023, 3, 1),
                                    End = new DateTime(2023, 3, 15),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 1",
                                    TaskGroupId = 8,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2],users[4]},

                                },
                                new Assignment{
                                    Id = 75,
                                    Title = "Customer Support",
                                    Owner = 2,
                                    Description = "Assist customers with inquiries",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 20,
                                    PriorityId = 4,
                                    Type = "Tip 3",
                                    TaskGroupId = 8,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2],users[8]},

                                },
                                new Assignment{
                                    Id = 76,
                                    Title = "Organize events",
                                    Owner = 2,
                                    Description = "Plan and execute company events",
                                    Start = new DateTime(2023, 3, 3),
                                    End = new DateTime(2023, 3, 20),
                                    StateId = 2,
                                    Percentage = 45,
                                    PriorityId = 4,
                                    Type = "Tip 2",
                                    TaskGroupId = 8,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0]},

                                },
                                //project 3 rok (2024, 3, 5) - jos traje,members:9,1,3,5
                                new Assignment{
                                    Id = 77,
                                    Title = "Maintain IT Systems",
                                    Owner = 1,
                                    Description = "Ensure the company`s IT infrastructure is secure",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 4, 14),
                                    StateId = 2,
                                    Percentage = 44,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 8,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[8],users[2]},

                                },
                                new Assignment{
                                    Id = 78,
                                    Title = "Prepare Financial Reports",
                                    Owner = 1,
                                    Description = "Compile and present financial data to assist with budgeting",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023,5, 14),
                                    StateId = 2,
                                    Percentage = 66,
                                    PriorityId = 3,
                                    Type = "Tip 3",
                                    TaskGroupId = 8,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2],users[4]},

                                },
                                new Assignment{
                                    Id = 79,
                                    Title = "Create Marketing Campaign",
                                    Owner = 1,
                                    Description = "Design and launch marketing initiatives to promote products and services",
                                    Start = new DateTime(2023, 3, 15),
                                    End = new DateTime(2023, 3, 25),
                                    StateId = 2,
                                    Percentage = 10,
                                    PriorityId = 1,
                                    Type = "Tip 1",
                                    TaskGroupId = 8,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[2],users[4]},

                                },
                                    //project 4 rok (2023, 4, 20) do (2023, 8, 31),members:2,4,5,7
                                new Assignment{
                                    Id = 80,
                                    Title = "Conduct Employee Training",
                                    Owner = 2,
                                    Description = "Develop and deliver training programs to enhance employee skils and knowledge",
                                    Start = new DateTime(2023, 4, 21),
                                    End = new DateTime(2023, 4, 30),
                                    StateId = 3,
                                    Percentage = 50,
                                    PriorityId = 1,
                                    Type = "Tip 2",
                                    TaskGroupId = 8,
                                    LastTimeChanged = DateTime.Now,
                                    IsClosed = false,
                                    Users = new List<User>{users[0],users[4],users[8]},

                                },

                            };

                            _context.AddRange(assignments);
                            _context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}