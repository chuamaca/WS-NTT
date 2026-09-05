USE BiblioNETS2DB;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Libro_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.LibroId,
        l.GeneroId,
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
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Libro_ObtenerPorId
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.LibroId,
        l.GeneroId,
        g.Nombre AS Genero,
        l.Titulo,
        l.Autor,
        l.ISBN,
        l.Stock
    FROM dbo.Libros AS l
    INNER JOIN dbo.Generos AS g
        ON g.GeneroId = l.GeneroId
    WHERE l.LibroId = @LibroId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Libro_Insertar
    @GeneroId INT,
    @Titulo NVARCHAR(150),
    @Autor NVARCHAR(120),
    @ISBN VARCHAR(20),
    @Stock INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Libros
        (GeneroId, Titulo, Autor, ISBN, Stock, Activo, FechaRegistro)
    VALUES
        (@GeneroId, @Titulo, @Autor, @ISBN, @Stock, 1, SYSDATETIME());

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Libro_Actualizar
    @LibroId INT,
    @GeneroId INT,
    @Titulo NVARCHAR(150),
    @Autor NVARCHAR(120),
    @ISBN VARCHAR(20),
    @Stock INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Libros
    SET
        GeneroId = @GeneroId,
        Titulo = @Titulo,
        Autor = @Autor,
        ISBN = @ISBN,
        Stock = @Stock
    WHERE LibroId = @LibroId;

    SELECT @@ROWCOUNT;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Libro_Eliminar
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Libros
    WHERE LibroId = @LibroId;

    SELECT @@ROWCOUNT;
END;
GO
