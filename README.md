# Entregable de sesiones [Chapter .NET]

# Programación orientada a objetos en C#
## Entregable I - Sistema de empleados
### Ruta: [1.Entregable I - Sistema de empleados](https://github.com/chuamaca/WS-NTT/tree/master/001-ProgramacionOrientadaObjetos/1.Entregable01-Sistema%20de%20empleados)
    Desarrollar una aplicación de consola en C#.
    La aplicación deberá tener las clases:
    •	Empleado.
    •	Developer
    •	TeamLeader.
    •	Manager.
    Aplicar:
    •	Herencia.
    •	Sobrescritura de métodos.
    •	Encapsulamiento.
    Ejemplo:
    CalcularBono()
    Cada tipo de empleado debe calcular el bono de forma distinta.
    Objetivo: aplicar herencia, sobrescritura y encapsulamiento con clases.

## Entregable II - Interfaces de notificación
### RUTA: [2.EntregableI-InterfaceNotificacion](https://github.com/chuamaca/WS-NTT/tree/master/001-ProgramacionOrientadaObjetos/2.Entregable02-InterfaceNotificacion)

    Desarrollar una aplicación de consola en C#.
    La aplicación deberá tener la interfaz:
    •	INotificador

    Implementaciones:
    •	EmailNoticador
    •	SmsNotificador
    •	TeamsNotificador

    Cada clase debe implementar un método Enviar().
    Objetivo: aplicar polimorfismo con interfaces.

## Entregable III – Delegados y eventos
### RUTA: [3.DelegadosYEventos](https://github.com/chuamaca/WS-NTT/tree/master/001-ProgramacionOrientadaObjetos/3.Entregable03-DelegadosYEventos)
    Desarrollar una aplicación de consola en C#.
    Crear un sistema de órdenes donde:
    •	Al crear una orden, se dispare un evento.
    •	El evento notifique por consola.
    •	Se use un delegado para procesar el mensaje.
    Objetivo: entender eventos y delegados en C#.

# Entregable II - SQL Server, ADO .NET, Entity Framework y Dapper

## Entregable I - Modelo de base de datos
### RUTA: [https://github.com/chuamaca/WS-NTT/tree/master/002-EfDapperADONETSQLServerBasico/001.Entregable01-Modelodebasededatos](https://github.com/chuamaca/WS-NTT/tree/master/002-EfDapperADONETSQLServerBasico/001.Entregable01-Modelodebasededatos)
    Crear base de datos SQL Server con las tablas:
    •	Productos.
    •	Categorias.
    •	Clientes.
    •	Ordenes.
    •	OrdenDetalle.
    Crear scripts:
    •	CREATE TABLE.
    •	INSERT.
    •	UPDATE.
    •	DELETE.
    •	SELECT.
    Objetivo: aplicar conocimientos de modelado de BD.

## Entregable II – ADO .NET
### RUTA: [[https://github.com/chuamaca/WS-NTT/tree/master/002-EfDapperADONETSQLServerBasico/001.Entregable01-Modelodebasededatos](https://github.com/chuamaca/WS-NTT/tree/master/002-EfDapperADONETSQLServerBasico/001.Entregable01-Modelodebasededatos)](https://github.com/chuamaca/WS-NTT/tree/master/002-EfDapperADONETSQLServerBasico/002.Entregable02-AdoNet)
    Desarrollar una aplicación(Consola, MVC o API) en C#.
    Crear una clase usando SqlConnection, SqlCommand y SqlDataReader.
    Que permita Ejecutar los siguientes métodos de la BD diseñada en el Entregable I:
    •	Consulta simple.
    •	INSERT.
    •	UPDATE.
    •	DELETE.
    Objetivo: aplicar conocimientos de ADO .NET.

## Entregable III – Entity Framework
### RUTA: [https://github.com/chuamaca/WS-NTT/tree/master/002-EfDapperADONETSQLServerBasico/003.Entregable03-EF](https://github.com/chuamaca/WS-NTT/tree/master/002-EfDapperADONETSQLServerBasico/003.Entregable03-EF)

    Desarrollar una aplicación(Consola, MVC o API) en C#.
    Crear las clases correspondientes para la configuración del Entity Framework.
    Que permita Ejecutar los siguientes métodos de la BD diseñada en el Entregable I:
    •	Consulta simple.
    •	INSERT.
    •	UPDATE.
    •	DELETE.
    Objetivo: aplicar conocimientos de Entity Framework.

## Entregable IV – DAPPER
### RUTA: [https://github.com/chuamaca/WS-NTT/tree/master/002-EfDapperADONETSQLServerBasico/003.Entregable03-EF](https://github.com/chuamaca/WS-NTT/tree/master/002-EfDapperADONETSQLServerBasico/004.Entregable04-Dapper)

    Desarrollar una aplicación(Consola, MVC o API) en C#.
    Crear las clases correspondientes para la configuración de Dapper.
    Que permita Ejecutar los siguientes métodos de la BD diseñada en el Entregable I:
    •	Consulta simple.
    •	INSERT.
    •	UPDATE.
    •	DELETE.
    Objetivo: aplicar conocimientos de Dapper.


# Documentación de una API REST con OpenAPI
## Entregable I -  Caso práctico: API de Gestión de Reservas de Salas
### Ruta: https://github.com/chuamaca/WS-NTT/tree/master/003-APIRESTOpenAPI/DemoApi

    Desarrollar una API denominada API de Gestión de Reservas de Salas, que permita administrar las reservas de salas de reuniones de una organización.
    Una reserva deberá manejar, como mínimo, la siguiente información:
        • Id
        • Nombre de la sala
        • Fecha de reserva
        • Hora de inicio
        • Hora de fin
        • Nombre del responsable
        • Cantidad de asistentes
        • Motivo de la reunión
        • Estado de la reserva (Pendiente, Confirmada o Cancelada)
    
    No es obligatorio implementar una base de datos. Se puede utilizar una colección en memoria para almacenar la información.
    
    Endpoints requeridos
    La API deberá implementar como mínimo los siguientes endpoints:
    Método	Endpoint	Descripción
    GET	/api/reservas	Obtener todas las reservas
    GET	/api/reservas/{id}	Obtener una reserva por su Id
    GET	/api/reservas?fecha={fecha}	Filtrar las reservas por fecha
    POST	/api/reservas	Registrar una nueva reserva
    PUT	/api/reservas/{id}	Actualizar una reserva existente
    DELETE	/api/reservas/{id}	Cancelar o eliminar una reserva
    
    Como parte de la implementación, se deberán considerar validaciones básicas, por ejemplo:
        • La hora de fin debe ser posterior a la hora de inicio.
        • La cantidad de asistentes debe ser mayor a cero.
        • Los datos obligatorios deben ser validados antes de registrar la reserva.
        • Se deberá responder apropiadamente cuando se consulte una reserva que no existe.
    
    Requerimientos de OpenAPI
    La API deberá generar una especificación OpenAPI correctamente configurada.
    La documentación deberá incluir:
        • Nombre, descripción y versión de la API.
        • Descripción de los endpoints.
        • Documentación de parámetros Path y Query.
        • Documentación del Request Body cuando corresponda.
        • Definición de los modelos utilizados.GetReserva
        • Uso de DTOs para Request y Response.
        • Documentación de los principales códigos HTTP que puede devolver cada operación.
    Como mínimo, deberán contemplarse cuando correspondan:
        • 200 OK
        • 201 Created
        • 400 Bad Request
        • 401 Unauthorized
        • 404 Not Found
    Se deberán utilizar XML Comments o un mecanismo equivalente para enriquecer la documentación generada.

## Entregable II  - Swagger UI
### Ruta: https://github.com/chuamaca/WS-NTT/blob/master/003-APIRESTOpenAPI/Dise%C3%B1oContrato/ContratoReservas.yaml

    La aplicación deberá tener habilitado Swagger UI y permitir:
        • Visualizar todos los endpoints implementados.
        • Consultar los parámetros requeridos por cada operación.
        • Visualizar los modelos de Request y Response.
        • Identificar los posibles códigos de respuesta.
        • Ejecutar los endpoints utilizando la opción Try it out.
    
    Seguridad
    Implementar y documentar en OpenAPI un esquema de autenticación mediante Bearer Token/JWT.
    Swagger UI deberá mostrar la opción Authorize, permitiendo ingresar un token para consumir al menos un endpoint protegido.
    No es necesario implementar un mecanismo complejo de autenticación; el objetivo principal es demostrar cómo se configura y documenta el esquema de seguridad.
    
## Entregable III - Generación de cliente
### Ruta: https://github.com/chuamaca/WS-NTT/tree/master/003-APIRESTOpenAPI/MiClienteReservas

    Utilizando la especificación OpenAPI generada por la API, generar un cliente .NET mediante alguna herramienta como:
        • NSwag, o
        • OpenAPI Generator.
    
    El participante deberá demostrar brevemente que el cliente generado contiene métodos asociados a los endpoints definidos en la API.    
