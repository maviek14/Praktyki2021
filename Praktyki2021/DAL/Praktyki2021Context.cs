using Praktyki2021.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Praktyki2021.DAL
{
    public class Praktyki2021Context : DbContext
    {
        public Praktyki2021Context()
            : base("name=MyDbConnection")
        { }

        public IDbSet<Device> Devices { get; set; }
        public IDbSet<Contract> Contracts { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>().ToTable("Device");
            modelBuilder.Entity<Contract>().ToTable("Contract");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Profile>().ToTable("Profile");
        }
    }
}