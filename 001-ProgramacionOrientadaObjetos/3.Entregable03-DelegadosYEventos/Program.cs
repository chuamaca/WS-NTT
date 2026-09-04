using _3.DelegadosYEventos.Delegate;
using _3.DelegadosYEventos.Objects;
using _3.DelegadosYEventos.Servicio;

// Paso 3: se usa un delegado para procesar el mensaje; "apunta" al metodo NotificarPorConsola.
var servicio = new ServicioNotificacion();
NotificarDelegado accion = servicio.NotificarPorConsola;

// El delegado se suscribe al evento de la orden.
Orden orden = new Orden(1001, "María González");
orden.OrdenCreada += accion;

// Paso 1: al crear la orden, se dispara el evento OrdenCreada.
orden.CrearOrden();
