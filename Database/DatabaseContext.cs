using Microsoft.EntityFrameworkCore;
using SmartSwitchWeb.Data;
using System;

namespace SmartSwitchWeb.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Message> Messages { get; set; }

        public string DbPath { get; }

        public DatabaseContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "SmartSwitch.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>().ToTable("Device");
            modelBuilder.Entity<Message>().ToTable("Message");
        }
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

}
