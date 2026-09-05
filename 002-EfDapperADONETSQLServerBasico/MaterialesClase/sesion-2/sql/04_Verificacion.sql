USE BiblioNETS2DB;
GO

SELECT COUNT(*) AS Registros
FROM dbo.LibrosBenchmarkSinIndice;

SELECT COUNT(*) AS Registros
FROM dbo.LibrosBenchmarkConIndice;

SELECT TOP 10 *
FROM dbo.LibrosBenchmarkSinIndice
ORDER BY LibroBenchmarkSinIndiceId;

SELECT TOP 10 *
FROM dbo.LibrosBenchmarkConIndice
ORDER BY LibroBenchmarkConIndiceId;
GO
