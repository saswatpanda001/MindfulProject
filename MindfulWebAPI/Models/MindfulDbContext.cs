using Microsoft.EntityFrameworkCore;

using MindfulWebAPI.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MindfulWebAPI.Models
{
    public class MindfulDbContext : DbContext
    {
        public MindfulDbContext(DbContextOptions<MindfulDbContext> options) : base(options)
        {

        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<MoodEntry> MoodEntries { get; set; }
        public DbSet<MeditationSession> MeditationSessions { get; set; }
        public DbSet<AffirmationEntry> AffirmationEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User fields
            modelBuilder.Entity<User>()
                 .HasIndex(u => u.Email)
                 .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.FullName)
                .IsUnique();


            modelBuilder.Entity<User>()
                .HasCheckConstraint("CK_User_FullName_MinLength", "LEN(FullName) >= 4");

            modelBuilder.Entity<User>()
                .HasCheckConstraint("CK_User_Email_MinLength", "LEN(Email) >= 4");

            modelBuilder.Entity<User>()
                .HasCheckConstraint("CK_User_Password_MinLength", "LEN(Password) >= 4");

            modelBuilder.Entity<User>()
                .HasCheckConstraint("CK_User_Location_MinLength", "LEN(Location) >= 4 OR Location IS NULL");

            modelBuilder.Entity<User>()
                .HasCheckConstraint("CK_User_Phone_ExactLength", "LEN(Phone) = 10");

            modelBuilder.Entity<MoodEntry>()
                .HasCheckConstraint("CK_MoodEntry_Mood_MinLength", "LEN(Mood) >= 3");

            modelBuilder.Entity<MoodEntry>()
                .HasCheckConstraint("CK_MoodEntry_Note_MinLength", "LEN(Note) >= 4");

            modelBuilder.Entity<MeditationSession>()
                .HasCheckConstraint("CK_MeditationSession_DurationMinutes_Range", "DurationMinutes >= 1 AND DurationMinutes <= 600");

            modelBuilder.Entity<AffirmationEntry>()
                .HasCheckConstraint("CK_AffirmationEntry_Category_MinLength", "LEN(Category) >= 4");

            modelBuilder.Entity<AffirmationEntry>()
                .HasCheckConstraint("CK_AffirmationEntry_Text_MinLength", "LEN(Text) >= 4");


            modelBuilder.Entity<MoodEntry>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade); 
            
            modelBuilder.Entity<MeditationSession>()
                .HasOne<User>().WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AffirmationEntry>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);


        }


    }
}
