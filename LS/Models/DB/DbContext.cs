using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Configuration;
using System.Text.RegularExpressions;

namespace MyMvc.Models.DB
{
    public class ModelContext : DbContext
    {
        private Regex rx = new Regex("([^@]+)(?:@(?:g|google)mail.com)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private ModelContext()
            : base(WebConfigurationManager.ConnectionStrings["MyDB"].ConnectionString)
        {
        }

        [ThreadStatic]
        private static ModelContext _instance;
        public static ModelContext Instance
        {
            get
            {
                if (_instance == null) _instance = new ModelContext();
                return _instance;
            }
        }

        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbDevice> Devices { get; set; }
        public DbSet<DbLocation> Locations { get; set; }
        public DbSet<DbThumbnail> Thumbnails { get; set; }
        public DbSet<DbLog> Logs { get; set; }
        public DbSet<DbRating> Ratings { get; set; }

        public DbSet<DbDeviceRelation> DeviceRelations { get; set; }
        public DbSet<DbDeviceMessage> DeviceMessages { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbUser>().ToTable("tblUsers");
            modelBuilder.Entity<DbDevice>().ToTable("tblDevices");
            modelBuilder.Entity<DbLocation>().ToTable("tblLocations");
            modelBuilder.Entity<DbThumbnail>().ToTable("tblThumbnails");

            modelBuilder.Entity<DbDeviceRelation>().ToTable("tblDeviceRelations");
            modelBuilder.Entity<DbDeviceMessage>().ToTable("tblDeviceMessages");

            modelBuilder.Entity<DbLog>().ToTable("tblLog");
            modelBuilder.Entity<DbRating>().ToTable("tblRating");
        }


        public DbUser FindUserByLogin(string login)
        {
            foreach (Match m in rx.Matches(login))
            {
                string name = m.Groups[1].Value;
                return ModelContext.Instance.Users.SingleOrDefault(x => x.Login == name || x.Login == name + "@gmail.com" || x.Login == name + "@googlemail.com");
            }
            return null;
        }
    }
}