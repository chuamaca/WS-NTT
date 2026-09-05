Create database fDapperADONETSQLServerBasico;
GO

USE fDapperADONETSQLServerBasico;
GO

CREATE TABLE dbo.Categorias
(
    IdCategoria INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    [State] BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    ModifiedAt DATETIME NULL,
    ModifiedBy NVARCHAR(100) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
);


CREATE TABLE dbo.Productos
(
    IdProducto INT PRIMARY KEY IDENTITY(1,1),
    IdCategoria INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Precio DECIMAL(18, 2) NOT NULL,
    Stock INT NOT NULL,
    [State] BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    ModifiedAt DATETIME NULL,
    ModifiedBy NVARCHAR(100) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (IdCategoria) REFERENCES dbo.Categorias(IdCategoria)
);

Alter table dbo.Productos
add Index IX_Productos_Nombre UNIQUE (Nombre);

Create Table dbo.Clientes
(
    IdCliente INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    Direccion NVARCHAR(200) NOT NULL,
    Documento NVARCHAR(20) NOT NULL,
    [State] BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    ModifiedAt DATETIME NULL,
    ModifiedBy NVARCHAR(100) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
);

alter table dbo.clientes
add Index IX_ClientesEmail UNIQUE (Email);
add Index IX_ClientesDocumento UNIQUE (Documento);
add Index IX_ClientesTelefono UNIQUE (Telefono);
add Index IX_ClientesNombreApellido UNIQUE (Nombre, Apellido);

Create table dbo.Ordenes
(
    IdOrden INT PRIMARY KEY IDENTITY(1,1),
    IdCliente INT NOT NULL,
    Serie NVARCHAR(50) NOT NULL,
    Comprobante NVARCHAR(50) NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(18, 2) NOT NULL,
    [State] BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    ModifiedAt DATETIME NULL,
    ModifiedBy NVARCHAR(100) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (IdCliente) REFERENCES dbo.Clientes(IdCliente)
);

Alter table dbo.Ordenes
add Index IX_OrdenesSerieComprobante UNIQUE (Serie, Comprobante);
add Index IX_OrdenesClienteIdFecha UNIQUE (IdCliente, Fecha);

Create table dbo.OrdenDetalles
(
    IdOrdenDetalles INT PRIMARY KEY IDENTITY(1,1),
    IdOrden INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    Precio DECIMAL(18, 2) NOT NULL,
    [State] BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    ModifiedAt DATETIME NULL,
    ModifiedBy NVARCHAR(100) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (IdOrden) REFERENCES dbo.Ordenes(IdOrden),
    FOREIGN KEY (IdProducto) REFERENCES dbo.Productos(IdProducto)
);
Alter table dbo.OrdenDetalles
add Index IX_OrdenDetallesOrdenIdProductoId UNIQUE (IdOrden, IdProducto);
add Index IX_OrdenDetallesProductoId UNIQUE (IdProducto);
add Index IX_OrdenDetallesOrdenId UNIQUE (IdOrden);


SET IDENTITY_INSERT dbo.Categorias ON;
Insert into dbo.Categorias (IdCategoria, Nombre, CreatedBy) VALUES (1, 'Bebidas', 'System');
Insert into dbo.Categorias (IdCategoria, Nombre, CreatedBy) VALUES (2, 'Comida', 'System');
Insert into dbo.Categorias (IdCategoria,Nombre, CreatedBy) values (3,'Postres', 'System');
SET IDENTITY_INSERT dbo.Categorias OFF;
go

Insert into dbo.Productos (IdCategoria, Nombre, Precio, Stock, CreatedBy)
values
(1, N'Coca Cola', 1.5, 100, N'System'),
(1, N'Pepsi', 1.4, 100, N'System'),
(1, N'Fanta', 1.3, 100, N'System'),
(2, N'Hamburguesa', 5.0, 50, N'System'),
(2, N'Pizza', 8.0, 50, N'System'),
(2, N'Ensalada', 4.0, 50, N'System');
go

Set IDENTITY_INSERT dbo.Clientes ON;
Insert into dbo.Clientes (IdCliente,Nombre, Apellido, Email, Telefono, Direccion, Documento, CreatedBy)
values 
(1,N'Juan', N'Perez', N'juanito@gmail.com',N'123456789',N'Calle Falsa 123',N'12345678',N'System'),
(2,N'Maria', N'Gomez', N'maria12344@gmail.com',N'987654321',N'Avenida Siempre Viva 456',N'87654321',N'System');
Set IDENTITY_INSERT dbo.Clientes OFF;
go

set IDENTITY_INSERT dbo.Ordenes ON;
Insert into dbo.Ordenes (IdOrden, IdCliente, Serie, Comprobante, Fecha, Total, CreatedBy)
values
(1, 1, N'A001', N'0001', GETDATE(), 10.0, N'System'),
(2, 2, N'A001', N'0002', GETDATE(), 15.0, N'System');
(3, 1, N'A001', N'0003', GETDATE(), 20.0, N'System');
(4, 2, N'A001', N'0004', GETDATE(), 25.0, N'System');
(5, 1, N'A001', N'0005', GETDATE(), 30.0, N'System');
Set IDENTITY_INSERT dbo.Ordenes OFF;
go

Insert into dbo.OrdenDetalles (IdOrden, IdProducto, Cantidad, Precio, CreatedBy)
values
(1, 1, 2, 1.5, N'System'),
(1, 4, 1, 5.0, N'System'),
(2, 2, 3, 1.4, N'System'),
(2, 5, 1, 8.0, N'System');
(2, 6, 1, 4.0, N'System');
(3, 3, 2, 1.3, N'System'),
(3, 4, 1, 5.0, N'System'),
(4, 5, 2, 8.0, N'System'),
(4, 6, 1, 4.0, N'System'),
(5, 1, 1, 1.5, N'System'),
(5, 2, 2, 1.4, N'System');

Update dbo.Clientes set Telefono = N'1234567890' where IdCliente = 1;
Update dbo.Clientes set Telefono = N'0987654321', Email = N'maria12345@gmail.com' where IdCliente = 2;
go

delete from Categorias where IdCategoria = 3;
go

Select
o.IdOrden, o.Serie, o.Comprobante, o.Fecha, o.Total, od.Cantidad, od.Precio, c.Nombre as NombreCliente, c.Apellido as ApellidoCliente, p.Nombre as NombreProducto, from dbo.Ordenes o inner join dbo.Clientes c on o.IdCliente = c.IdCliente
inner join dbo.OrdenDetalles od on o.IdOrden = od.IdOrden
inner join dbo.Productos p on od.IdProducto = p.IdProducto
Where o.IsDeleted = 1 and c.IsDeleted = 1 and od.IsDeleted = 1 and p.IsDeleted = 1;


select 
o.IdOrden, o.Serie, o.Comprobante, o.Fecha, o.Total, od.Cantidad, od.Precio, c.Nombre as NombreCliente, c.Apellido as ApellidoCliente, p.Nombre as NombreProducto
from dbo.Ordenes o  
inner join dbo.Clientes c on o.IdCliente = c.IdCliente
inner join dbo.OrdenDetalles od on o.IdOrden = od.IdOrden
inner join dbo.Productos p on od.IdProducto = p.IdProducto
where c.IdCliente = 1 and o.IsDeleted = 0 and c.IsDeleted = 0 and od.IsDeleted = 0 and p.IsDeleted = 0;







