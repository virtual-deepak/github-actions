using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class MusicCatalogContext : DbContext
{
    public MusicCatalogContext (DbContextOptions<MusicCatalogContext> options)
        : base(options)
    {
    }

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>().ToTable("Artist");
        modelBuilder.Entity<Album>().ToTable("Album");
    }
}