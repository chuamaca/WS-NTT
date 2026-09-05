using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BiblioNET.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    GeneroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.GeneroId);
                });

            migrationBuilder.CreateTable(
                name: "Lectores",
                columns: table => new
                {
                    LectorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Documento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectores", x => x.LectorId);
                });

            migrationBuilder.CreateTable(
                name: "LibrosBenchmarkConIndice",
                columns: table => new
                {
                    LibroBenchmarkConIndiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneroId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    ISBN = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibrosBenchmarkConIndice", x => x.LibroBenchmarkConIndiceId);
                });

            migrationBuilder.CreateTable(
                name: "LibrosBenchmarkSinIndice",
                columns: table => new
                {
                    LibroBenchmarkSinIndiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneroId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    ISBN = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibrosBenchmarkSinIndice", x => x.LibroBenchmarkSinIndiceId);
                });

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    LibroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneroId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    ISBN = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.LibroId);
                    table.CheckConstraint("CK_Libros_Stock", "[Stock] >= 0");
                    table.ForeignKey(
                        name: "FK_Libros_Generos_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Generos",
                        principalColumn: "GeneroId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    PrestamoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LectorId = table.Column<int>(type: "int", nullable: false),
                    FechaPrestamo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.PrestamoId);
                    table.CheckConstraint("CK_Prestamos_Estado", "[Estado] IN ('ACTIVO','DEVUELTO','VENCIDO')");
                    table.CheckConstraint("CK_Prestamos_Fechas", "[FechaVencimiento] >= [FechaPrestamo]");
                    table.ForeignKey(
                        name: "FK_Prestamos_Lectores_LectorId",
                        column: x => x.LectorId,
                        principalTable: "Lectores",
                        principalColumn: "LectorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrestamoDetalle",
                columns: table => new
                {
                    PrestamoDetalleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrestamoId = table.Column<int>(type: "int", nullable: false),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<short>(type: "smallint", nullable: false),
                    Devuelto = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestamoDetalle", x => x.PrestamoDetalleId);
                    table.CheckConstraint("CK_PrestamoDetalle_Cantidad", "[Cantidad] > 0");
                    table.ForeignKey(
                        name: "FK_PrestamoDetalle_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "LibroId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrestamoDetalle_Prestamos_PrestamoId",
                        column: x => x.PrestamoId,
                        principalTable: "Prestamos",
                        principalColumn: "PrestamoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "GeneroId", "Activo", "Descripcion", "FechaRegistro", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Obras narrativas de ficción", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Novela" },
                    { 2, true, "Programación, arquitectura y tecnología", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tecnología" },
                    { 3, true, "Historia universal y regional", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Historia" },
                    { 4, true, "Divulgación y ciencias aplicadas", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ciencia" }
                });

            migrationBuilder.InsertData(
                table: "Lectores",
                columns: new[] { "LectorId", "Activo", "Apellidos", "Documento", "Email", "FechaRegistro", "Nombres" },
                values: new object[,]
                {
                    { 1, true, "Torres", "74000001", "ana.torres@example.com", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ana" },
                    { 2, true, "Ramírez", "74000002", "luis.ramirez@example.com", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luis" }
                });

            migrationBuilder.InsertData(
                table: "Libros",
                columns: new[] { "LibroId", "Activo", "Autor", "FechaRegistro", "GeneroId", "ISBN", "Stock", "Titulo" },
                values: new object[,]
                {
                    { 1, true, "Gabriel García Márquez", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "9780307474728", 4, "Cien años de soledad" },
                    { 2, true, "Robert C. Martin", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "9780132350884", 3, "Clean Code" },
                    { 3, true, "Martin Kleppmann", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "9781449373320", 2, "Designing Data-Intensive Applications" },
                    { 4, true, "Yuval Noah Harari", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "9780062316097", 5, "Sapiens" },
                    { 5, true, "Stephen Hawking", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "9780553380163", 1, "A Brief History of Time" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Generos_Nombre",
                table: "Generos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lectores_Documento",
                table: "Lectores",
                column: "Documento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Libros_GeneroId",
                table: "Libros",
                column: "GeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_ISBN",
                table: "Libros",
                column: "ISBN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Stock",
                table: "Libros",
                column: "Stock");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Titulo",
                table: "Libros",
                column: "Titulo");

            migrationBuilder.CreateIndex(
                name: "IX_LibrosBenchmarkConIndice_GeneroId_Stock_Activo",
                table: "LibrosBenchmarkConIndice",
                columns: new[] { "GeneroId", "Stock", "Activo" })
                .Annotation("SqlServer:Include", new[] { "Titulo", "Autor", "ISBN" });

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoDetalle_LibroId",
                table: "PrestamoDetalle",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoDetalle_PrestamoId_LibroId",
                table: "PrestamoDetalle",
                columns: new[] { "PrestamoId", "LibroId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_LectorId_FechaPrestamo",
                table: "Prestamos",
                columns: new[] { "LectorId", "FechaPrestamo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibrosBenchmarkConIndice");

            migrationBuilder.DropTable(
                name: "LibrosBenchmarkSinIndice");

            migrationBuilder.DropTable(
                name: "PrestamoDetalle");

            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "Prestamos");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropTable(
                name: "Lectores");
        }
    }
}
