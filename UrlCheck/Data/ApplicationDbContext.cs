using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlCheck.Models;

namespace UrlCheck.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<UrlCheckLog> UrlCheckLogs { get; set; } = null!;
        public virtual DbSet<UrlList> UrlLists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UrlCheckLog>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUserEmail)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.UrlList)
                    .WithMany(p => p.UrlCheckLogs)
                    .HasForeignKey(d => d.UrlListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UrlCheckLogs_UrlLists");
            });

            modelBuilder.Entity<UrlList>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUserEmail)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}