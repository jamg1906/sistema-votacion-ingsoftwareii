using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace final_softwareii_API.Models;

public partial class FinalSoftwareiiContext : DbContext
{
    public FinalSoftwareiiContext()
    {
    }

    public FinalSoftwareiiContext(DbContextOptions<FinalSoftwareiiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candidato> Candidatos { get; set; }

    public virtual DbSet<Estadistica> Estadisticas { get; set; }

    public virtual DbSet<Fase> Fases { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Voto> Votos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)

        {

            IConfigurationRoot configuration = new ConfigurationBuilder()

            .SetBasePath(Directory.GetCurrentDirectory())

                        .AddJsonFile("appsettings.json")

                        .Build();

            var connectionString = configuration.GetConnectionString("VotacionBD");

            optionsBuilder.UseMySQL(connectionString);

        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidato>(entity =>
        {
            entity.HasKey(e => e.Dpi).HasName("PRIMARY");

            entity.ToTable("candidato");

            entity.Property(e => e.Dpi).HasColumnName("DPI");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("date")
                .HasColumnName("fechaNacimiento");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(100)
                .HasColumnName("nombreCompleto");
            entity.Property(e => e.PartidoPolitico)
                .HasMaxLength(100)
                .HasColumnName("partidoPolitico");
        });

        modelBuilder.Entity<Estadistica>(entity =>
        {
            entity.HasKey(e => e.Idestadisticas).HasName("PRIMARY");

            entity.ToTable("estadisticas");

            entity.Property(e => e.Idestadisticas)
                .HasMaxLength(100)
                .HasColumnName("idestadisticas");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
        });

        modelBuilder.Entity<Fase>(entity =>
        {
            entity.HasKey(e => e.Idfases).HasName("PRIMARY");

            entity.ToTable("fases");

            entity.Property(e => e.Idfases).HasColumnName("idfases");
            entity.Property(e => e.Estado).HasColumnName("estado");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Correo).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .HasColumnName("contraseña");
        });

        modelBuilder.Entity<Voto>(entity =>
        {
            entity.HasKey(e => e.DpiVotante).HasName("PRIMARY");

            entity.ToTable("voto");

            entity.HasIndex(e => e.DpiCandidato, "DPI_idx");

            entity.Property(e => e.DpiVotante).HasColumnName("dpiVotante");
            entity.Property(e => e.DpiCandidato).HasColumnName("dpiCandidato");
            entity.Property(e => e.HoraVoto)
                .HasColumnType("datetime")
                .HasColumnName("horaVoto");
            entity.Property(e => e.IpVoto)
                .HasMaxLength(100)
                .HasColumnName("ipVoto");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
