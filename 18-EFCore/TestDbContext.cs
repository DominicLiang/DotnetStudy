﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _18_EFCore;
public class TestDbContext : DbContext
{
    // 代码显示SQL 控制台日志
    private static ILoggerFactory loggerFactory = LoggerFactory.Create(b => b.AddConsole());

    public DbSet<Book> Books { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Dog> Dogs { get; set; }
    public DbSet<Cat> Cats { get; set; }
    public DbSet<House> houses { get; set; }
    public DbSet<Like> likes { get; set; }
    public DbSet<Shop> shops { get; set; }

    public string DbPath { get; }

    public TestDbContext()
    {
        var folder = Environment.SpecialFolder.DesktopDirectory;
        string path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "TestDB.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite($"Data Source={DbPath}");

        // 代码显示SQL
        optionsBuilder.UseLoggerFactory(loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //从当前程序集加载所有的IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}