using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<InstructorProfile> InstructorProfiles => Set<InstructorProfile>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Admin> Admins => Set<Admin>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── One-to-One: Instructor ↔ InstructorProfile ──────────────────────
        modelBuilder.Entity<Instructor>()
            .HasOne(i => i.Profile)
            .WithOne(p => p.Instructor)
            .HasForeignKey<InstructorProfile>(p => p.InstructorId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── One-to-Many: Instructor → Courses ───────────────────────────────
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── Many-to-Many: Student ↔ Course via Enrollment ───────────────────
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint: a student can only enroll once per course
        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.StudentId, e.CourseId })
            .IsUnique();

        // Unique emails
        modelBuilder.Entity<Instructor>().HasIndex(i => i.Email).IsUnique();
        modelBuilder.Entity<Student>().HasIndex(s => s.Email).IsUnique();
        modelBuilder.Entity<Admin>().HasIndex(a => a.Email).IsUnique();

        // ── Seed Data ────────────────────────────────────────────────────────
    
    }
}
