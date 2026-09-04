using _2.EntregableI_InterfaceNotificacion.Interface;

namespace _2.EntregableI_InterfaceNotificacion.Implement;

public class TeamsNotificador : INotificador
{
    public void Enviar()
    {
        Console.WriteLine("Enviando notificación por Teams...");
    }
}
