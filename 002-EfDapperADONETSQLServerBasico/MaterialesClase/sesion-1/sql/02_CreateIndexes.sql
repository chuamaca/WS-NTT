USE BiblioNETDB;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = N'IX_Libros_GeneroId'
      AND object_id = OBJECT_ID(N'dbo.Libros')
)
    CREATE INDEX IX_Libros_GeneroId ON dbo.Libros(GeneroId);
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = N'IX_Libros_Titulo'
      AND object_id = OBJECT_ID(N'dbo.Libros')
)
    CREATE INDEX IX_Libros_Titulo ON dbo.Libros(Titulo);
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = N'IX_Libros_Stock'
      AND object_id = OBJECT_ID(N'dbo.Libros')
)
    CREATE INDEX IX_Libros_Stock ON dbo.Libros(Stock);
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = N'IX_Prestamos_LectorId_FechaPrestamo'
      AND object_id = OBJECT_ID(N'dbo.Prestamos')
)
    CREATE INDEX IX_Prestamos_LectorId_FechaPrestamo
        ON dbo.Prestamos(LectorId, FechaPrestamo);
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = N'IX_PrestamoDetalle_LibroId'
      AND object_id = OBJECT_ID(N'dbo.PrestamoDetalle')
)
    CREATE INDEX IX_PrestamoDetalle_LibroId
        ON dbo.PrestamoDetalle(LibroId);
GO
