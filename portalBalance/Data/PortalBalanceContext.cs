using Microsoft.EntityFrameworkCore;
using portalBalance.Models;
using System.Collections.Generic;
using System.Linq;

namespace portalBalance.Data
{
    public class PortalBalanceContext : DbContext
    {
        public PortalBalanceContext(DbContextOptions<PortalBalanceContext> options)
            : base(options)
        {
        }

        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<OrgAdmin> OrgAdmins { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<SubOrg> SubOrgs { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ProfessorCourse> ProfessorCourses { get; set; }
        public DbSet<PaymentFile> PaymentFiles { get; set; }
        public DbSet<CourseTransaction> CourseTransactions { get; set; }
        public DbSet<ProfessorTransaction> ProfessorTransaction { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }
        public DbSet<UniversityExpense> UniversityExpenses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            modelBuilder.Entity<UniversityExpense>().ToTable("UniversityExpenses");

            modelBuilder.Entity<Professor>()
               .HasIndex(p => p.NationalID)
               .IsUnique();

                    modelBuilder.Entity<ProfessorCourse>()
                        .HasKey(pc => new { pc.ProfessorId, pc.CourseId });

                    modelBuilder.Entity<ProfessorCourse>()
                        .HasOne(pc => pc.Professor)
                        .WithMany(p => p.ProfessorCourses)
                        .HasForeignKey(pc => pc.ProfessorId);

                    modelBuilder.Entity<ProfessorCourse>()
                        .HasOne(pc => pc.Course)
                        .WithMany(c => c.ProfessorCourses)
                        .HasForeignKey(pc => pc.CourseId);

                    modelBuilder.Entity<Organization>()
                        .HasMany(o => o.OrgAdmins)
                        .WithOne(a => a.Organization)
                        .HasForeignKey(a => a.OrganizationId)
                        .OnDelete(DeleteBehavior.Cascade);

                    modelBuilder.Entity<Organization>()
                        .HasMany(o => o.SubOrgs)
                        .WithOne(s => s.Organization)
                        .HasForeignKey(s => s.OrganizationId)
                        .OnDelete(DeleteBehavior.Cascade);

                    modelBuilder.Entity<SubOrg>()
                        .HasOne(so => so.Organization)
                        .WithMany(o => o.SubOrgs)
                        .HasForeignKey(so => so.OrganizationId)
                        .OnDelete(DeleteBehavior.Restrict);

                    modelBuilder.Entity<Department>()
                        .HasOne(d => d.SubOrg)
                        .WithMany(so => so.Departments)
                        .HasForeignKey(d => d.SubOrgId)
                        .OnDelete(DeleteBehavior.Restrict);

                    modelBuilder.Entity<Course>()
                        .HasOne(c => c.Department)
                        .WithMany(d => d.Courses)
                        .HasForeignKey(c => c.DepartmentId)
                        .OnDelete(DeleteBehavior.Restrict);

                    modelBuilder.Entity<PaymentFile>(entity =>
                    {
                        entity.HasKey(e => e.Id);
                        entity.Property(e => e.File).IsRequired();
                        entity.Property(e => e.Organization).IsRequired();
                        entity.Property(e => e.UploaderName).IsRequired();
                        entity.Property(e => e.Date).IsRequired();
                        entity.Property(e => e.Time).IsRequired();
                        entity.Property(e => e.TotalIncome).IsRequired();
                        entity.Property(e => e.Status).IsRequired();
                    });

                    modelBuilder.Entity<CourseTransaction>()
                        .HasOne(ct => ct.Course)
                        .WithMany(c => c.Transactions)
                        .HasForeignKey(ct => ct.CourseId);

                    modelBuilder.Entity<ProfessorTransaction>()
                        .HasOne(pt => pt.Professor)
                        .WithMany(p => p.ProfessorTransactions)
                        .HasForeignKey(pt => pt.NationalId)
                        .HasPrincipalKey(p => p.NationalID);

                    modelBuilder.Entity<ProfessorTransaction>()
                        .Property(t => t.Status)
                        .HasConversion<string>();

                    modelBuilder.Entity<ProfessorTransaction>()
                        .Property(t => t.Type)
                        .HasConversion<string>();

                    base.OnModelCreating(modelBuilder);
                }
            }
        }


