USE BiblioNETDB;
GO

-- SELECT directo con JOIN
SELECT
    l.LibroId,
    g.Nombre AS Genero,
    l.Titulo,
    l.Autor,
    l.ISBN,
    l.Stock
FROM dbo.Libros AS l
INNER JOIN dbo.Generos AS g
    ON g.GeneroId = l.GeneroId
WHERE l.Activo = 1
ORDER BY l.Titulo;
GO

-- Ejemplo controlado para comparar DML directo.
-- En C# estos valores se envían usando SqlParameter.
INSERT INTO dbo.Libros (GeneroId, Titulo, Autor, ISBN, Stock)
VALUES (4, N'Libro temporal SQL', N'Autor Demo', 'DEMO-SQL-001', 1);

DECLARE @LibroDirectoId INT = SCOPE_IDENTITY();

UPDATE dbo.Libros
SET Stock = 2
WHERE LibroId = @LibroDirectoId;

DELETE FROM dbo.Libros
WHERE LibroId = @LibroDirectoId;
GO
