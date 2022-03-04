using System;
using Data.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Base;

public class DbContextFixture : IDisposable
{
    private const string ConnectionString =
        "Server = .;Database=Object18_DB_Beta_Test;Trusted_Connection=True;MultipleActiveResultSets=true";

    private SqlConnection SqlConnection { get; }

    static DbContextFixture()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MainDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;
        var dbContext = new MainDbContext(dbContextOptions);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        dbContext.Dispose();
    }

    public DbContextFixture()
    {
        SqlConnection = new SqlConnection(ConnectionString);
        SqlConnection.Open();
        var dbContextOptions = new DbContextOptionsBuilder<MainDbContext>()
            .UseSqlServer(SqlConnection)
            .Options;

        var dbContext = new MainDbContext(dbContextOptions);
        dbContext.Database.ExecuteSqlRaw(@"
                EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
                EXEC sp_MSForEachTable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'
                EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'
            ");
    }
    public MainDbContext CreateNewDbContext()
    {
        var dbContextOptions =
            new DbContextOptionsBuilder<MainDbContext>()
                .UseSqlServer(SqlConnection, _ =>
                {
                    //b.AddBulkOperationSupport();
                })
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

        var dbContext = new MainDbContext(dbContextOptions);
        return dbContext;
    }

    public virtual void Dispose()
    {
        SqlConnection.Close();
        SqlConnection.Dispose();
    }
}