using Praktyki2021.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Praktyki2021.DAL
{
    public class Praktyki2021Initializer : DropCreateDatabaseIfModelChanges<Praktyki2021Context>
    {
        protected override void Seed(Praktyki2021Context context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));

            var user = new ApplicationUser { UserName = "admin@gmail.com", Email= "admin@gmail.com" };
            string password = "zaq1@WSX";
            userManager.Create(user, password);
            userManager.AddToRole(user.Id, "Admin");

            var user1 = new ApplicationUser { UserName = "maciek.trochymiuk@gmail.com", Email = "maciek.trochymiuk@gmail.com" };
            string password1 = "zaq1@WSX";
            userManager.Create(user1, password1);
            userManager.AddToRole(user1.Id, "User");

            var user2 = new ApplicationUser { UserName = "tomek.nowak@gmail.com", Email = "tomek.nowak@gmail.com" };
            string password2 = "zaq1@WSX";
            userManager.Create(user2, password2);
            userManager.AddToRole(user2.Id, "User");

            var user3 = new ApplicationUser { UserName = "karolina.sulej@gmail.com", Email = "karolina.sulej@gmail.com" };
            string password3 = "zaq1@WSX";
            userManager.Create(user3, password3);
            userManager.AddToRole(user3.Id, "User");

            var profiles = new List<Profile>
            {
                new Profile{UserName=user.UserName},
                new Profile{UserName=user1.UserName},
                new Profile{UserName=user2.UserName},
                new Profile{UserName=user3.UserName}
            };
            profiles.ForEach(_ => context.Profiles.Add(_));
            context.SaveChanges();

            var users = new List<User>
            {
                new User{
                    Name="ADMIN", 
                    Surname="ADMIN", 
                    AddedTime=DateTime.Parse("2020-01-01"), 
                    Profile=profiles[0]
                },
                new User{
                    Name="Maciek", 
                    Surname="Trochymiuk", 
                    AddedTime=DateTime.Parse("2021-01-01"), 
                    Profile=profiles[1]
                },
                new User{
                    Name="Tomek", 
                    Surname="Nowak", 
                    AddedTime=DateTime.Parse("2021-01-01"), 
                    Profile=profiles[2]
                },
                new User{
                    Name="Karolina", 
                    Surname="Sulej", 
                    AddedTime=DateTime.Parse("2021-01-01"), 
                    Profile=profiles[3]
                }
            };
            users.ForEach(_ => context.Users.Add(_));
            context.SaveChanges();

            var devices = new List<Device>
            {
                new Device{
                    Type=DeviceType.PC, 
                    Name="Komputer 2009", 
                    Description="Mocarny komputer", 
                    Profile=profiles[1]
                },
                new Device{
                    Type=DeviceType.Monitor, 
                    Name="Monitor MSI", 
                    Description="144hz 1ms", 
                    Profile=profiles[1]
                },
                new Device{
                    Type=DeviceType.Charger, 
                    Name="Ładowarka 1", 
                    Description="apple", 
                    Profile=profiles[2]
                }
            };
            devices.ForEach(_ => context.Devices.Add(_));
            context.SaveChanges();

            var contracts = new List<Contract>
            {
                new Contract{
                    Name="Napraw ktoś kompa!!",
                    Description="Nie włącza sie",
                    Price=400,
                    Status=ContractStatus.Available,
                    CreatedTime=DateTime.Parse("2021-04-30"),
                    Device=devices[0],
                    Principal=profiles[1]
                },
                new Contract{
                    Name="Zepsuty monitor!!1!",
                    Description="Ciemny ekran po wlaczeniu",
                    Price=30,
                    Status=ContractStatus.Completed,
                    CreatedTime=DateTime.Parse("2021-04-30"),
                    Device=devices[1],
                    Principal=profiles[1],
                    CompletedTime=DateTime.Parse("2021-04-30"),
                    Mandatory=profiles[3]
                }
            };
            contracts.ForEach(_ => context.Contracts.Add(_));
            context.SaveChanges();
        }
    }
}