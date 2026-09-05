USE BiblioNETDB;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Genero_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT GeneroId, Nombre
    FROM dbo.Generos
    WHERE Activo = 1
    ORDER BY Nombre;
END;
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
        l.Stock,
        l.Activo
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
        l.Stock,
        l.Activo
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

    INSERT INTO dbo.Libros (GeneroId, Titulo, Autor, ISBN, Stock)
    VALUES (@GeneroId, @Titulo, @Autor, @ISBN, @Stock);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS LibroId;
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

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Libro_Eliminar
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM dbo.PrestamoDetalle
        WHERE LibroId = @LibroId
    )
    BEGIN
        THROW 50001, 'No se puede eliminar el libro porque tiene préstamos asociados.', 1;
    END;

    DELETE FROM dbo.Libros
    WHERE LibroId = @LibroId;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO
