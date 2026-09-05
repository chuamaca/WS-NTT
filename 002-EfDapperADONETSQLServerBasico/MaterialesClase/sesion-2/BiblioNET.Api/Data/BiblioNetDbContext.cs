using BiblioNET.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace BiblioNET.Api.Data;

public sealed class BiblioNetDbContext(DbContextOptions<BiblioNetDbContext> options)
    : DbContext(options)
{
    public DbSet<Genero> Generos => Set<Genero>();
    public DbSet<Libro> Libros => Set<Libro>();
    public DbSet<Lector> Lectores => Set<Lector>();
    public DbSet<Prestamo> Prestamos => Set<Prestamo>();
    public DbSet<PrestamoDetalle> PrestamoDetalles => Set<PrestamoDetalle>();

    public DbSet<LibroBenchmarkSinIndice> LibrosBenchmarkSinIndice
        => Set<LibroBenchmarkSinIndice>();

    public DbSet<LibroBenchmarkConIndice> LibrosBenchmarkConIndice
        => Set<LibroBenchmarkConIndice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigurarGeneros(modelBuilder);
        ConfigurarLibros(modelBuilder);
        ConfigurarLectores(modelBuilder);
        ConfigurarPrestamos(modelBuilder);
        ConfigurarPrestamoDetalle(modelBuilder);
        ConfigurarBenchmark(modelBuilder);
        SembrarDatos(modelBuilder);
    }

    private static void ConfigurarGeneros(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Genero>();

        entity.ToTable("Generos");
        entity.HasKey(x => x.GeneroId);

        entity.Property(x => x.Nombre)
            .HasMaxLength(80)
            .IsRequired();

        entity.Property(x => x.Descripcion)
            .HasMaxLength(250);

        entity.HasIndex(x => x.Nombre)
            .IsUnique();
    }

    private static void ConfigurarLibros(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Libro>();

        entity.ToTable("Libros");
        entity.HasKey(x => x.LibroId);

        entity.Property(x => x.Titulo)
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(x => x.Autor)
            .HasMaxLength(120)
            .IsRequired();

        entity.Property(x => x.ISBN)
            .HasMaxLength(20)
            .IsUnicode(false)
            .IsRequired();

        entity.HasIndex(x => x.ISBN)
            .IsUnique();

        entity.HasIndex(x => x.GeneroId);
        entity.HasIndex(x => x.Titulo);
        entity.HasIndex(x => x.Stock);

        entity.HasOne(x => x.Genero)
            .WithMany(x => x.Libros)
            .HasForeignKey(x => x.GeneroId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.ToTable(t =>
            t.HasCheckConstraint("CK_Libros_Stock", "[Stock] >= 0"));
    }

    private static void ConfigurarLectores(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Lector>();

        entity.ToTable("Lectores");
        entity.HasKey(x => x.LectorId);

        entity.Property(x => x.Documento)
            .HasMaxLength(20)
            .IsUnicode(false)
            .IsRequired();

        entity.Property(x => x.Nombres)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(x => x.Apellidos)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(x => x.Email)
            .HasMaxLength(150);

        entity.HasIndex(x => x.Documento)
            .IsUnique();
    }

    private static void ConfigurarPrestamos(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Prestamo>();

        entity.ToTable("Prestamos");
        entity.HasKey(x => x.PrestamoId);

        entity.Property(x => x.Estado)
            .HasMaxLength(15)
            .IsUnicode(false);

        entity.HasIndex(x => new { x.LectorId, x.FechaPrestamo });

        entity.HasOne(x => x.Lector)
            .WithMany(x => x.Prestamos)
            .HasForeignKey(x => x.LectorId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_Prestamos_Fechas",
                "[FechaVencimiento] >= [FechaPrestamo]");

            t.HasCheckConstraint(
                "CK_Prestamos_Estado",
                "[Estado] IN ('ACTIVO','DEVUELTO','VENCIDO')");
        });
    }

    private static void ConfigurarPrestamoDetalle(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PrestamoDetalle>();

        entity.ToTable("PrestamoDetalle");
        entity.HasKey(x => x.PrestamoDetalleId);

        entity.HasIndex(x => new { x.PrestamoId, x.LibroId })
            .IsUnique();

        entity.HasIndex(x => x.LibroId);

        entity.HasOne(x => x.Prestamo)
            .WithMany(x => x.Detalles)
            .HasForeignKey(x => x.PrestamoId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Libro)
            .WithMany(x => x.PrestamoDetalles)
            .HasForeignKey(x => x.LibroId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.ToTable(t =>
            t.HasCheckConstraint(
                "CK_PrestamoDetalle_Cantidad",
                "[Cantidad] > 0"));
    }

    private static void ConfigurarBenchmark(ModelBuilder modelBuilder)
    {
        var sinIndice = modelBuilder.Entity<LibroBenchmarkSinIndice>();

        sinIndice.ToTable("LibrosBenchmarkSinIndice");
        sinIndice.HasKey(x => x.LibroBenchmarkSinIndiceId);

        sinIndice.Property(x => x.Titulo).HasMaxLength(150).IsRequired();
        sinIndice.Property(x => x.Autor).HasMaxLength(120).IsRequired();
        sinIndice.Property(x => x.ISBN).HasMaxLength(20).IsUnicode(false).IsRequired();

        // Intencionalmente NO se crea un índice secundario para GeneroId + Stock.

        var conIndice = modelBuilder.Entity<LibroBenchmarkConIndice>();

        conIndice.ToTable("LibrosBenchmarkConIndice");
        conIndice.HasKey(x => x.LibroBenchmarkConIndiceId);

        conIndice.Property(x => x.Titulo).HasMaxLength(150).IsRequired();
        conIndice.Property(x => x.Autor).HasMaxLength(120).IsRequired();
        conIndice.Property(x => x.ISBN).HasMaxLength(20).IsUnicode(false).IsRequired();

        // Esta es la única diferencia relevante del experimento.
        conIndice.HasIndex(x => new
        {
            x.GeneroId,
            x.Stock,
            x.Activo
        })
        .HasDatabaseName(
            "IX_LibrosBenchmarkConIndice_GeneroId_Stock_Activo")
        .IncludeProperties(x => new
        {
            x.Titulo,
            x.Autor,
            x.ISBN
        });
    }

    private static void SembrarDatos(ModelBuilder modelBuilder)
    {
        var fecha = new DateTime(2026, 8, 30);

        modelBuilder.Entity<Genero>().HasData(
            new Genero { GeneroId = 1, Nombre = "Novela", Descripcion = "Obras narrativas de ficción", Activo = true, FechaRegistro = fecha },
            new Genero { GeneroId = 2, Nombre = "Tecnología", Descripcion = "Programación, arquitectura y tecnología", Activo = true, FechaRegistro = fecha },
            new Genero { GeneroId = 3, Nombre = "Historia", Descripcion = "Historia universal y regional", Activo = true, FechaRegistro = fecha },
            new Genero { GeneroId = 4, Nombre = "Ciencia", Descripcion = "Divulgación y ciencias aplicadas", Activo = true, FechaRegistro = fecha }
        );

        modelBuilder.Entity<Libro>().HasData(
            new Libro { LibroId = 1, GeneroId = 1, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", ISBN = "9780307474728", Stock = 4, Activo = true, FechaRegistro = fecha },
            new Libro { LibroId = 2, GeneroId = 2, Titulo = "Clean Code", Autor = "Robert C. Martin", ISBN = "9780132350884", Stock = 3, Activo = true, FechaRegistro = fecha },
            new Libro { LibroId = 3, GeneroId = 2, Titulo = "Designing Data-Intensive Applications", Autor = "Martin Kleppmann", ISBN = "9781449373320", Stock = 2, Activo = true, FechaRegistro = fecha },
            new Libro { LibroId = 4, GeneroId = 3, Titulo = "Sapiens", Autor = "Yuval Noah Harari", ISBN = "9780062316097", Stock = 5, Activo = true, FechaRegistro = fecha },
            new Libro { LibroId = 5, GeneroId = 4, Titulo = "A Brief History of Time", Autor = "Stephen Hawking", ISBN = "9780553380163", Stock = 1, Activo = true, FechaRegistro = fecha }
        );

        modelBuilder.Entity<Lector>().HasData(
            new Lector { LectorId = 1, Documento = "74000001", Nombres = "Ana", Apellidos = "Torres", Email = "ana.torres@example.com", Activo = true, FechaRegistro = fecha },
            new Lector { LectorId = 2, Documento = "74000002", Nombres = "Luis", Apellidos = "Ramírez", Email = "luis.ramirez@example.com", Activo = true, FechaRegistro = fecha }
        );
    }
}
