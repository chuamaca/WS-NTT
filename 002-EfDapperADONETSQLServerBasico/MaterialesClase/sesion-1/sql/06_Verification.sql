USE BiblioNETDB;
GO

SELECT * FROM dbo.Generos ORDER BY GeneroId;
SELECT * FROM dbo.Libros ORDER BY LibroId;
SELECT * FROM dbo.Lectores ORDER BY LectorId;
SELECT * FROM dbo.Prestamos ORDER BY PrestamoId;
SELECT * FROM dbo.PrestamoDetalle ORDER BY PrestamoDetalleId;
GO

EXEC dbo.usp_Libro_Listar;
GO
