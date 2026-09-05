USE BiblioNETDB;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Generos)
BEGIN
    INSERT INTO dbo.Generos (Nombre, Descripcion)
    VALUES
        (N'Novela', N'Obras narrativas de ficción'),
        (N'Tecnología', N'Programación, arquitectura y tecnología'),
        (N'Historia', N'Historia universal y regional'),
        (N'Ciencia', N'Divulgación y ciencias aplicadas');
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Libros)
BEGIN
    INSERT INTO dbo.Libros (GeneroId, Titulo, Autor, ISBN, Stock)
    VALUES
        (1, N'Cien años de soledad', N'Gabriel García Márquez', '9780307474728', 4),
        (2, N'Clean Code', N'Robert C. Martin', '9780132350884', 3),
        (2, N'Designing Data-Intensive Applications', N'Martin Kleppmann', '9781449373320', 2),
        (3, N'Sapiens', N'Yuval Noah Harari', '9780062316097', 5),
        (4, N'A Brief History of Time', N'Stephen Hawking', '9780553380163', 2);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Lectores)
BEGIN
    INSERT INTO dbo.Lectores (Documento, Nombres, Apellidos, Email)
    VALUES
        ('74000001', N'Ana', N'Torres', N'ana.torres@example.com'),
        ('74000002', N'Luis', N'Ramírez', N'luis.ramirez@example.com');
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Prestamos)
BEGIN
    INSERT INTO dbo.Prestamos (LectorId, FechaPrestamo, FechaVencimiento, Estado)
    VALUES (1, CAST(GETDATE() AS DATE), DATEADD(DAY, 7, CAST(GETDATE() AS DATE)), 'ACTIVO');

    DECLARE @PrestamoId INT = SCOPE_IDENTITY();

    INSERT INTO dbo.PrestamoDetalle (PrestamoId, LibroId, Cantidad, Devuelto)
    VALUES (@PrestamoId, 2, 1, 0);
END;
GO
