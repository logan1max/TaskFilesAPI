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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@$"Server=localhost\SQLEXPRESS;Database={DatabaseConstants.TaskFilesDatabaseName};Trusted_Connection=True;");
        }
    }
}
