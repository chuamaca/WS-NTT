using BiblioNET.Consola.Data;
using BiblioNET.Consola.Models;
using Microsoft.Data.SqlClient;

namespace BiblioNET.Consola;

internal static class Program
{
    private static readonly LibroSqlDirectoDataAccess SqlDirecto = new();
    private static readonly LibroStoredProcedureDataAccess StoredProcedures = new();

    public static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine("              BiblioNET               ");
            Console.WriteLine("======================================");
            Console.WriteLine("Modo de acceso a datos:");
            Console.WriteLine("1. SQL directo");
            Console.WriteLine("2. Stored Procedures");
            Console.WriteLine("0. Salir");
            Console.Write("\nSeleccione una opción: ");

            var opcion = Console.ReadLine();

            if (opcion == "0")
                return;

            if (opcion is not ("1" or "2"))
            {
                Pausa("Opción inválida.");
                continue;
            }

            await MostrarMenuCrudAsync(opcion == "2");
        }
    }

    private static async Task MostrarMenuCrudAsync(bool usarStoredProcedures)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine($" BiblioNET · {(usarStoredProcedures ? "Stored Procedures" : "SQL directo")}");
            Console.WriteLine("======================================");
            Console.WriteLine("1. Listar libros");
            Console.WriteLine("2. Buscar libro por ID");
            Console.WriteLine("3. Insertar libro");
            Console.WriteLine("4. Actualizar libro");
            Console.WriteLine("5. Eliminar libro");
            Console.WriteLine("0. Volver");
            Console.Write("\nSeleccione una opción: ");

            var opcion = Console.ReadLine();

            if (opcion == "0")
                return;

            try
            {
                switch (opcion)
                {
                    case "1":
                        await ListarAsync(usarStoredProcedures);
                        break;
                    case "2":
                        await BuscarAsync(usarStoredProcedures);
                        break;
                    case "3":
                        await InsertarAsync(usarStoredProcedures);
                        break;
                    case "4":
                        await ActualizarAsync(usarStoredProcedures);
                        break;
                    case "5":
                        await EliminarAsync(usarStoredProcedures);
                        break;
                    default:
                        Pausa("Opción inválida.");
                        break;
                }
            }
            catch (SqlException ex)
            {
                Pausa($"SQL Server respondió con un error:\n{ex.Message}");
            }
            catch (Exception ex)
            {
                Pausa($"Ocurrió un error:\n{ex.Message}");
            }
        }
    }

    private static async Task ListarAsync(bool usarStoredProcedures)
    {
        var libros = usarStoredProcedures
            ? await StoredProcedures.ListarAsync()
            : await SqlDirecto.ListarAsync();

        Console.Clear();
        Console.WriteLine($"{"ID",-5} {"GÉNERO",-16} {"TÍTULO",-38} {"AUTOR",-28} {"STOCK",5}");
        Console.WriteLine(new string('-', 100));

        foreach (var libro in libros)
        {
            Console.WriteLine(
                $"{libro.LibroId,-5} " +
                $"{Recortar(libro.Genero, 16),-16} " +
                $"{Recortar(libro.Titulo, 38),-38} " +
                $"{Recortar(libro.Autor, 28),-28} " +
                $"{libro.Stock,5}");
        }

        Pausa($"\nTotal: {libros.Count} libro(s).");
    }

    private static async Task BuscarAsync(bool usarStoredProcedures)
    {
        var libroId = LeerEntero("LibroId: ");

        var libro = usarStoredProcedures
            ? await StoredProcedures.ObtenerPorIdAsync(libroId)
            : await SqlDirecto.ObtenerPorIdAsync(libroId);

        Console.Clear();

        if (libro is null)
        {
            Pausa("Libro no encontrado.");
            return;
        }

        Console.WriteLine($"ID:      {libro.LibroId}");
        Console.WriteLine($"Género:  {libro.Genero} ({libro.GeneroId})");
        Console.WriteLine($"Título:  {libro.Titulo}");
        Console.WriteLine($"Autor:   {libro.Autor}");
        Console.WriteLine($"ISBN:    {libro.ISBN}");
        Console.WriteLine($"Stock:   {libro.Stock}");
        Console.WriteLine($"Activo:  {libro.Activo}");

        Pausa();
    }

    private static async Task InsertarAsync(bool usarStoredProcedures)
    {
        Console.Clear();
        Console.WriteLine("NUEVO LIBRO\n");

        var input = LeerLibro();

        var nuevoId = usarStoredProcedures
            ? await StoredProcedures.InsertarAsync(input)
            : await SqlDirecto.InsertarAsync(input);

        Pausa($"Libro insertado correctamente. Nuevo LibroId: {nuevoId}");
    }

    private static async Task ActualizarAsync(bool usarStoredProcedures)
    {
        Console.Clear();
        Console.WriteLine("ACTUALIZAR LIBRO\n");

        var libroId = LeerEntero("LibroId a actualizar: ");
        var input = LeerLibro();

        var filas = usarStoredProcedures
            ? await StoredProcedures.ActualizarAsync(libroId, input)
            : await SqlDirecto.ActualizarAsync(libroId, input);

        Pausa(filas > 0
            ? $"Actualización completada. Filas afectadas: {filas}"
            : "No se encontró el LibroId indicado.");
    }

    private static async Task EliminarAsync(bool usarStoredProcedures)
    {
        Console.Clear();
        Console.WriteLine("ELIMINAR LIBRO\n");

        var libroId = LeerEntero("LibroId a eliminar: ");

        Console.Write($"¿Confirma eliminar el libro {libroId}? (S/N): ");
        var confirmar = Console.ReadLine()?.Trim().ToUpperInvariant();

        if (confirmar != "S")
        {
            Pausa("Operación cancelada.");
            return;
        }

        var filas = usarStoredProcedures
            ? await StoredProcedures.EliminarAsync(libroId)
            : await SqlDirecto.EliminarAsync(libroId);

        Pausa(filas > 0
            ? $"Libro eliminado. Filas afectadas: {filas}"
            : "No se encontró el LibroId indicado.");
    }

    private static LibroInput LeerLibro()
    {
        return new LibroInput
        {
            GeneroId = LeerEntero("GeneroId: "),
            Titulo = LeerTexto("Título: "),
            Autor = LeerTexto("Autor: "),
            ISBN = LeerTexto("ISBN: "),
            Stock = LeerEntero("Stock: ")
        };
    }

    private static int LeerEntero(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);

            if (int.TryParse(Console.ReadLine(), out var valor))
                return valor;

            Console.WriteLine("Ingrese un número entero válido.");
        }
    }

    private static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            var valor = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(valor))
                return valor;

            Console.WriteLine("El valor es obligatorio.");
        }
    }

    private static string Recortar(string valor, int maximo)
    {
        if (valor.Length <= maximo)
            return valor;

        return valor[..(maximo - 3)] + "...";
    }

    private static void Pausa(string? mensaje = null)
    {
        if (!string.IsNullOrWhiteSpace(mensaje))
            Console.WriteLine($"\n{mensaje}");

        Console.WriteLine("\nPresione ENTER para continuar...");
        Console.ReadLine();
    }
}
