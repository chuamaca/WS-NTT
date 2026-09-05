USE BiblioNETS2DB;
GO

DECLARE @Cantidad INT = 1000000;

DELETE FROM dbo.LibrosBenchmarkSinIndice;
DELETE FROM dbo.LibrosBenchmarkConIndice;

DBCC CHECKIDENT ('dbo.LibrosBenchmarkSinIndice', RESEED, 0);
DBCC CHECKIDENT ('dbo.LibrosBenchmarkConIndice', RESEED, 0);

;WITH Numeros AS
(
    SELECT TOP (@Cantidad)
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS N
    FROM sys.all_objects AS a
    CROSS JOIN sys.all_objects AS b
)
INSERT INTO dbo.LibrosBenchmarkSinIndice
(
    GeneroId,
    Titulo,
    Autor,
    ISBN,
    Stock,
    Activo
)
SELECT
    ((N - 1) % 20) + 1,
    CONCAT(N'Libro benchmark ', N),
    CONCAT(N'Autor ', ((N - 1) % 5000) + 1),
    CONCAT('BENCH-', RIGHT(REPLICATE('0', 12) + CAST(N AS VARCHAR(12)), 12)),
    (N * 17) % 100,
    1
FROM Numeros;

SET IDENTITY_INSERT dbo.LibrosBenchmarkConIndice ON;

INSERT INTO dbo.LibrosBenchmarkConIndice
(
    LibroBenchmarkConIndiceId,
    GeneroId,
    Titulo,
    Autor,
    ISBN,
    Stock,
    Activo
)
SELECT
    LibroBenchmarkSinIndiceId,
    GeneroId,
    Titulo,
    Autor,
    ISBN,
    Stock,
    Activo
FROM dbo.LibrosBenchmarkSinIndice;

SET IDENTITY_INSERT dbo.LibrosBenchmarkConIndice OFF;

SELECT
    (SELECT COUNT(*) FROM dbo.LibrosBenchmarkSinIndice) AS SinIndice,
    (SELECT COUNT(*) FROM dbo.LibrosBenchmarkConIndice) AS ConIndice;
GO
