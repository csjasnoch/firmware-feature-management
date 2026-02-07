using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<NpiProgram> NpiPrograms => Set<NpiProgram>();
    public DbSet<FirmwareVersion> FirmwareVersions => Set<FirmwareVersion>();
    public DbSet<ParameterDefinition> ParameterDefinitions => Set<ParameterDefinition>();
    public DbSet<CommandDefinition> CommandDefinitions => Set<CommandDefinition>();
    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<FeatureCollection> FeatureCollections => Set<FeatureCollection>();
    public DbSet<OperationalFlow> OperationalFlows => Set<OperationalFlow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // NpiProgram configuration
        modelBuilder.Entity<NpiProgram>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasMany(e => e.FirmwareVersions)
                .WithOne(fv => fv.NpiProgram)
                .HasForeignKey(fv => fv.NpiProgramId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // FirmwareVersion configuration
        modelBuilder.Entity<FirmwareVersion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Version).IsRequired().HasMaxLength(50);
            entity.HasMany(e => e.Parameters)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Commands)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ParameterDefinition configuration
        modelBuilder.Entity<ParameterDefinition>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        });

        // CommandDefinition configuration
        modelBuilder.Entity<CommandDefinition>(entity =>
        {
            entity.HasKey(e => e.CommandCode);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasMany(e => e.Parameters)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Feature configuration
        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.OwnsMany(e => e.Parameters, pb =>
            {
                pb.Property(p => p.ParameterName).HasMaxLength(200);
            });
            entity.OwnsMany(e => e.Commands, cb =>
            {
                cb.Property(c => c.CommandName).HasMaxLength(200);
            });
        });

        // FeatureCollection configuration
        modelBuilder.Entity<FeatureCollection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasOne(e => e.TargetProgram)
                .WithMany()
                .HasForeignKey(e => e.NpiProgramId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.RequiredLpiVersion)
                .WithMany()
                .HasForeignKey(e => e.RequiredLpiVersionId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.RequiredPiccoloVersion)
                .WithMany()
                .HasForeignKey(e => e.RequiredPiccoloVersionId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Features)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        });

        // OperationalFlow configuration
        modelBuilder.Entity<OperationalFlow>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.OwnsMany(e => e.Operations, ob =>
            {
                ob.Property(o => o.Name).HasMaxLength(200);
                ob.OwnsMany(o => o.Steps, sb =>
                {
                    sb.Property(s => s.Title).HasMaxLength(200);
                    sb.Property(s => s.AttributesJson).HasColumnName("Attributes");
                    sb.OwnsMany(s => s.Branches);
                });
            });
        });
    }
}
