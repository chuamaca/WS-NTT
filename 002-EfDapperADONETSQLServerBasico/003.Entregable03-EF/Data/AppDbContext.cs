using _003.Entregable03_EF.Domain;
using Microsoft.EntityFrameworkCore;

namespace _003.Entregable03_EF.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Orden> Ordenes => Set<Orden>();
    public DbSet<OrdenDetalle> OrdenDetalles => Set<OrdenDetalle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigurarCategorias(modelBuilder);
        ConfigurarProductos(modelBuilder);
        ConfigurarClientes(modelBuilder);
        ConfigurarOrdenes(modelBuilder);
        ConfigurarOrdenDetalles(modelBuilder);
    }

    private static void ConfigurarCategorias(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Categoria>();

        entity.ToTable("Categorias", "dbo");
        entity.HasKey(x => x.IdCategoria);

        entity.Property(x => x.IdCategoria).HasColumnName("IdCategoria").ValueGeneratedOnAdd();
        entity.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        entity.Property(x => x.State).HasColumnName("State").HasDefaultValue(true);
        entity.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasDefaultValueSql("GETDATE()");
        entity.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();
        entity.Property(x => x.ModifiedBy).HasMaxLength(100);
        entity.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);
    }

    private static void ConfigurarProductos(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Producto>();

        entity.ToTable("Productos", "dbo");
        entity.HasKey(x => x.IdProducto);

        entity.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        entity.Property(x => x.Precio).HasColumnType("decimal(18,2)");
        entity.Property(x => x.State).HasDefaultValue(true);
        entity.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        entity.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();
        entity.Property(x => x.ModifiedBy).HasMaxLength(100);
        entity.Property(x => x.IsDeleted).HasDefaultValue(false);

        entity.HasIndex(x => x.Nombre).IsUnique();

        entity.HasOne(x => x.Categoria)
            .WithMany(x => x.Productos)
            .HasForeignKey(x => x.IdCategoria)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigurarClientes(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Cliente>();

        entity.ToTable("Clientes", "dbo");
        entity.HasKey(x => x.IdCliente);

        entity.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        entity.Property(x => x.Apellido).HasMaxLength(100).IsRequired();
        entity.Property(x => x.Email).HasMaxLength(100).IsRequired();
        entity.Property(x => x.Telefono).HasMaxLength(20).IsRequired();
        entity.Property(x => x.Direccion).HasMaxLength(200).IsRequired();
        entity.Property(x => x.Documento).HasMaxLength(20).IsRequired();
        entity.Property(x => x.State).HasDefaultValue(true);
        entity.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        entity.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();
        entity.Property(x => x.ModifiedBy).HasMaxLength(100);
        entity.Property(x => x.IsDeleted).HasDefaultValue(false);

        entity.HasIndex(x => x.Email).IsUnique();
        entity.HasIndex(x => x.Documento).IsUnique();
        entity.HasIndex(x => x.Telefono).IsUnique();
        entity.HasIndex(x => new { x.Nombre, x.Apellido }).IsUnique();
    }

    private static void ConfigurarOrdenes(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Orden>();

        entity.ToTable("Ordenes", "dbo");
        entity.HasKey(x => x.IdOrden);

        entity.Property(x => x.Serie).HasMaxLength(50).IsRequired();
        entity.Property(x => x.Comprobante).HasMaxLength(50).IsRequired();
        entity.Property(x => x.Fecha).HasDefaultValueSql("GETDATE()");
        entity.Property(x => x.Total).HasColumnType("decimal(18,2)");
        entity.Property(x => x.State).HasDefaultValue(true);
        entity.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        entity.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();
        entity.Property(x => x.ModifiedBy).HasMaxLength(100);
        entity.Property(x => x.IsDeleted).HasDefaultValue(false);

        entity.HasIndex(x => new { x.Serie, x.Comprobante }).IsUnique();
        entity.HasIndex(x => new { x.IdCliente, x.Fecha }).IsUnique();

        entity.HasOne(x => x.Cliente)
            .WithMany(x => x.Ordenes)
            .HasForeignKey(x => x.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigurarOrdenDetalles(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<OrdenDetalle>();

        entity.ToTable("OrdenDetalles", "dbo");
        entity.HasKey(x => x.IdOrdenDetalles);

        entity.Property(x => x.Precio).HasColumnType("decimal(18,2)");
        entity.Property(x => x.State).HasDefaultValue(true);
        entity.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        entity.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();
        entity.Property(x => x.ModifiedBy).HasMaxLength(100);
        entity.Property(x => x.IsDeleted).HasDefaultValue(false);

        entity.HasIndex(x => new { x.IdOrden, x.IdProducto }).IsUnique();
        entity.HasIndex(x => x.IdProducto);
        entity.HasIndex(x => x.IdOrden);

        entity.HasOne(x => x.Orden)
            .WithMany(x => x.OrdenDetalles)
            .HasForeignKey(x => x.IdOrden)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Producto)
            .WithMany(x => x.OrdenDetalles)
            .HasForeignKey(x => x.IdProducto)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
