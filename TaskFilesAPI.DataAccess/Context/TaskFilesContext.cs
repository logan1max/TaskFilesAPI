using TaskFilesAPI.DataAccess.Context.Entities;

namespace TaskFilesAPI.DataAccess.Context;

public class TaskFilesContext : DbContext
{
    public TaskFilesContext()
    {
    }

    public TaskFilesContext(DbContextOptions<TaskFilesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TaskModel> Tasks { get; set; } = null!;
    public virtual DbSet<FileModel> Files { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@$"Server=localhost\SQLEXPRESS;Database={DatabaseConstants.TaskFilesDatabaseName};User Id=sa;Password=12345;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskModel>(entity =>
        {
            entity.Property(e => e.TaskId)
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<FileModel>(entity =>
        {
            entity.Property(e => e.FileId)
                .ValueGeneratedNever();

            entity
                .HasOne(e => e.Task)
                .WithMany(p => p.Files)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_file_relations_task");
        });
    }
}
