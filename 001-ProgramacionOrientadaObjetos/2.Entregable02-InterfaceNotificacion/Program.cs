using _2.EntregableI_InterfaceNotificacion.Implement;
using _2.EntregableI_InterfaceNotificacion.Interface;

System.Console.WriteLine("------------------- Lista de notificaciones -------------------");
  
List<INotificador> notificadores = new()
{
    new EmailNoticador(),
    new SmsNotificador(),
    new TeamsNotificador()
};


foreach (INotificador notificador in notificadores)
{
    notificador.Enviar();
}

System.Console.WriteLine("------------------- Instancia a cada notificador -------------------");

//Instancia a cada notificador
var notificadorEmail = new EmailNoticador();
notificadorEmail.Enviar();

var notificadorSms = new SmsNotificador();
notificadorSms.Enviar();

var notificadorTeams = new TeamsNotificador();
notificadorTeams.Enviar();
