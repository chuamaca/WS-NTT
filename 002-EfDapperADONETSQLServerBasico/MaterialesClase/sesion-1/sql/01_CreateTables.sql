USE BiblioNETDB;
GO

IF OBJECT_ID(N'dbo.Generos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Generos
    (
        GeneroId       INT IDENTITY(1,1) NOT NULL,
        Nombre         NVARCHAR(80) NOT NULL,
        Descripcion    NVARCHAR(250) NULL,
        Activo         BIT NOT NULL CONSTRAINT DF_Generos_Activo DEFAULT (1),
        FechaRegistro  DATETIME2(0) NOT NULL CONSTRAINT DF_Generos_FechaRegistro DEFAULT (SYSDATETIME()),

        CONSTRAINT PK_Generos PRIMARY KEY (GeneroId),
        CONSTRAINT UQ_Generos_Nombre UNIQUE (Nombre)
    );
END;
GO

IF OBJECT_ID(N'dbo.Libros', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Libros
    (
        LibroId        INT IDENTITY(1,1) NOT NULL,
        GeneroId       INT NOT NULL,
        Titulo         NVARCHAR(150) NOT NULL,
        Autor          NVARCHAR(120) NOT NULL,
        ISBN           VARCHAR(20) NOT NULL,
        Stock          INT NOT NULL CONSTRAINT DF_Libros_Stock DEFAULT (0),
        Activo         BIT NOT NULL CONSTRAINT DF_Libros_Activo DEFAULT (1),
        FechaRegistro  DATETIME2(0) NOT NULL 
		CONSTRAINT DF_Libros_FechaRegistro DEFAULT (SYSDATETIME()),

        CONSTRAINT PK_Libros PRIMARY KEY (LibroId),
        CONSTRAINT UQ_Libros_ISBN UNIQUE (ISBN),
        CONSTRAINT FK_Libros_Generos
            FOREIGN KEY (GeneroId) REFERENCES dbo.Generos(GeneroId),
        CONSTRAINT CK_Libros_Stock CHECK (Stock >= 0)
    );
END;
GO

IF OBJECT_ID(N'dbo.Lectores', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Lectores
    (
        LectorId       INT IDENTITY(1,1) NOT NULL,
        Documento      VARCHAR(20) NOT NULL,
        Nombres        NVARCHAR(100) NOT NULL,
        Apellidos      NVARCHAR(100) NOT NULL,
        Email          NVARCHAR(150) NULL,
        Activo         BIT NOT NULL CONSTRAINT DF_Lectores_Activo DEFAULT (1),
        FechaRegistro  DATETIME2(0) NOT NULL CONSTRAINT DF_Lectores_FechaRegistro DEFAULT (SYSDATETIME()),

        CONSTRAINT PK_Lectores PRIMARY KEY (LectorId),
        CONSTRAINT UQ_Lectores_Documento UNIQUE (Documento)
    );
END;
GO

IF OBJECT_ID(N'dbo.Prestamos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Prestamos
    (
        PrestamoId       INT IDENTITY(1,1) NOT NULL,
        LectorId         INT NOT NULL,
        FechaPrestamo    DATE NOT NULL,
        FechaVencimiento DATE NOT NULL,
        Estado           VARCHAR(15) NOT NULL
            CONSTRAINT DF_Prestamos_Estado DEFAULT ('ACTIVO'),

        CONSTRAINT PK_Prestamos PRIMARY KEY (PrestamoId),
        CONSTRAINT FK_Prestamos_Lectores
            FOREIGN KEY (LectorId) REFERENCES dbo.Lectores(LectorId),
        CONSTRAINT CK_Prestamos_Fechas
            CHECK (FechaVencimiento >= FechaPrestamo),
        CONSTRAINT CK_Prestamos_Estado
            CHECK (Estado IN ('ACTIVO', 'DEVUELTO', 'VENCIDO'))
    );
END;
GO

IF OBJECT_ID(N'dbo.PrestamoDetalle', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PrestamoDetalle
    (
        PrestamoDetalleId INT IDENTITY(1,1) NOT NULL,
        PrestamoId        INT NOT NULL,
        LibroId           INT NOT NULL,
        Cantidad          SMALLINT NOT NULL
            CONSTRAINT DF_PrestamoDetalle_Cantidad DEFAULT (1),
        Devuelto          BIT NOT NULL
            CONSTRAINT DF_PrestamoDetalle_Devuelto DEFAULT (0),

        CONSTRAINT PK_PrestamoDetalle PRIMARY KEY (PrestamoDetalleId),
        CONSTRAINT FK_PrestamoDetalle_Prestamos
            FOREIGN KEY (PrestamoId) REFERENCES dbo.Prestamos(PrestamoId),
        CONSTRAINT FK_PrestamoDetalle_Libros
            FOREIGN KEY (LibroId) REFERENCES dbo.Libros(LibroId),
        CONSTRAINT UQ_PrestamoDetalle_Prestamo_Libro
            UNIQUE (PrestamoId, LibroId),
        CONSTRAINT CK_PrestamoDetalle_Cantidad
            CHECK (Cantidad > 0)
    );
END;
GO
