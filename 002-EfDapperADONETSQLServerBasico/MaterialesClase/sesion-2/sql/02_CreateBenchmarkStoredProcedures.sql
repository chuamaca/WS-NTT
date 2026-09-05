USE BiblioNETS2DB;
GO

/* ============================================
   COUNT - SIN ÍNDICE SECUNDARIO
   ============================================ */

CREATE OR ALTER PROCEDURE
    dbo.usp_BenchmarkLibros_SinIndice_Contar
    @GeneroId INT,
    @Stock INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT_BIG(*)
    FROM dbo.LibrosBenchmarkSinIndice
    WHERE GeneroId = @GeneroId
      AND Stock = @Stock
      AND Activo = 1;
END;
GO


/* ============================================
   COUNT - CON ÍNDICE
   ============================================ */

CREATE OR ALTER PROCEDURE
    dbo.usp_BenchmarkLibros_ConIndice_Contar
    @GeneroId INT,
    @Stock INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT_BIG(*)
    FROM dbo.LibrosBenchmarkConIndice
    WHERE GeneroId = @GeneroId
      AND Stock = @Stock
      AND Activo = 1;
END;
GO


/* ============================================
   CONSULTA REAL - SIN ÍNDICE
   ============================================ */

CREATE OR ALTER PROCEDURE
    dbo.usp_BenchmarkLibros_SinIndice_Consultar
    @GeneroId INT,
    @Stock INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (100)
        LibroBenchmarkSinIndiceId AS LibroId,
        GeneroId,
        Titulo,
        Autor,
        ISBN,
        Stock
    FROM dbo.LibrosBenchmarkSinIndice
    WHERE GeneroId = @GeneroId
      AND Stock = @Stock
      AND Activo = 1;
END;
GO


/* ============================================
   CONSULTA REAL - CON ÍNDICE
   ============================================ */

CREATE OR ALTER PROCEDURE
    dbo.usp_BenchmarkLibros_ConIndice_Consultar
    @GeneroId INT,
    @Stock INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (100)
        LibroBenchmarkConIndiceId AS LibroId,
        GeneroId,
        Titulo,
        Autor,
        ISBN,
        Stock
    FROM dbo.LibrosBenchmarkConIndice
    WHERE GeneroId = @GeneroId
      AND Stock = @Stock
      AND Activo = 1;
END;
GO