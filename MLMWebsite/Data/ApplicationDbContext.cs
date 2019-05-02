using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MLMWebsite.Models;
using MLMWebsite.ViewModel;

namespace MLMWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Proof>()
                .HasOne<ApplicationUser>(a => a.ApplicationUser)
                .WithMany(d => d.Proofs)
                .HasForeignKey(d => d.ApplicationMemberId);

            builder.Entity<LoginEntry>()
                .HasOne<ApplicationUser>(a => a.ApplicationUser)
                .WithMany(d => d.LoginEntries)
                .HasForeignKey(d => d.ApplicationMemberId);

            builder.Entity<AddressProof>()
                .HasOne<ApplicationUser>(a => a.ApplicationUser)
                .WithOne(d => d.AddressProofs)
                .HasForeignKey<AddressProof>(d => d.UserId);

            builder.Entity<UserChild>()
               .HasOne<ApplicationUser>(a => a.ApplicationUser)
               .WithMany(d => d.UserChildren)
               .HasForeignKey(d => d.ParentUserId);

            builder.Entity<UserAssets>()
            .HasOne<ApplicationUser>(a => a.ApplicationUser)
            .WithOne(d => d.UserAssets)
            .HasForeignKey<UserAssets>(d => d.UserId);

            builder.Entity<BarCode>()
            .HasOne<ApplicationUser>(a => a.ApplicationUser)
            .WithOne(d => d.BarCodes)
            .HasForeignKey<BarCode>(d => d.UserId);

           
        }

        public DbSet<LevelSetting> LevelSettings { get; set; }
        public DbSet<MLMWebsite.Models.Proof> Proof { get; set; }
        public DbSet<MLMWebsite.Models.LoginEntry> LoginEntries { get; set; }
        public DbSet<UserChild> UserChildren { get; set; }
        public DbSet<UserAssets> UserAssets { get; set; }
        public DbSet<AddressProof> AddressProofs { get; set; }
        public DbSet<BarCode> BarCodes { get; set; }
        public DbSet<DummyAdminAccount> DummyAdminAccounts { get; set; }
        public DbSet<ApprovedUser> ApprovedUsers { get; set; }



        public DbSet<MLMWebsite.Models.UserStatus> UserStatus { get; set; }
    }
}
