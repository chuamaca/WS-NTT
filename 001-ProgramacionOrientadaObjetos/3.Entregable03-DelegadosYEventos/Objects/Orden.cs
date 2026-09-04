namespace _3.DelegadosYEventos.Objects;

using _3.DelegadosYEventos.Delegate;

public class Orden
{
    public int Numero { get; }
    public string Cliente { get; }

    // Paso 1: se declara el evento que se disparara al crear la orden.
    public event NotificarDelegado? OrdenCreada;

    public Orden(int numero, string cliente)
    {
        Numero = numero;
        Cliente = cliente;
    }

    public void CrearOrden()
    {
        Console.WriteLine("Creando orden...");

        string mensaje = $"La orden #{Numero} para el cliente {Cliente} fue creada.";

        // Paso 1: al crear la orden, se dispara el evento (ejecuta el/los metodos suscritos con +=).
        // Paso 2 y 3: el evento invoca el delegado suscrito, que procesa el mensaje y notifica por consola.
        OrdenCreada?.Invoke(mensaje);
    }
}
