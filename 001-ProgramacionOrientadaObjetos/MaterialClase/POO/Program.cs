/*PROGRAMACIÓN ORIENTADA A OBJETOS*/

//====================================== 1. POO ====================================================
/*CuentaBancaria cuenta = new CuentaBancaria("Ana Torres", 1000);
cuenta.Depositar(500);
Console.WriteLine(cuenta.Saldo); // 1500

cuenta.Retirar(300);
Console.WriteLine(cuenta.Saldo); // 1200

cuenta.Retirar(1300);
Console.WriteLine(cuenta.Saldo); // Excepción
cuenta.Saldo = 999999;  //No compila: el 'set' de Saldo es private
cuenta.Titular = "";    //Lanza excepción: la validación lo rechaza
public class CuentaBancaria
{
    // ---------- CAMPOS ----------
    private string _titular;
    private decimal _saldo;

    // ---------- PROPIEDADES ----------
    public string Titular
    {
        get { return _titular; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El titular no puede estar vacío.");
            _titular = value;
        }
    }

    // Propiedad de solo lectura desde fuera: se puede leer el saldo,
    public decimal Saldo
    {
        get { return _saldo; }
        private set {

            if(value < 0)
                throw new ArgumentException("El saldo no puede ser negativo.");
            
            _saldo = value; 
        }
    }

    // ---------- CONSTRUCTOR ----------
    public CuentaBancaria(string titular, decimal saldoInicial)
    {
        Titular = titular; // reutiliza la validación de la propiedad
        Saldo = saldoInicial;
    }

    // ---------- MÉTODOS PÚBLICOS (comportamiento controlado) ----------
    public void Depositar(decimal monto)
    {
        Saldo += monto;
    }

    public void Retirar(decimal monto)
    {
        if (monto > Saldo)
            throw new InvalidOperationException("Fondos insuficientes.");

        Saldo -= monto;
    }
}*/

//====================================== 2. HERENCIA, POLIMORFISMO-SOBRESCRITURA DE MÉTODOS ====================================================
/*var empleado = new Empleado();
empleado.Nombre = "David Rivero";

var vendedor = new Vendedor();
vendedor.Nombre = "Eder García";
vendedor.VentasMes = 1000;

var gerente = new Gerente();
gerente.Nombre = "José Mujica";

Console.WriteLine(empleado.Nombre);
Console.WriteLine(empleado.CalcularBono());

Console.WriteLine(vendedor.Nombre);
Console.WriteLine(vendedor.CalcularBono());

Console.WriteLine(gerente.Nombre);
Console.WriteLine(gerente.CalcularBono());

public class Empleado
{
    public string Nombre { get; set; }

    public virtual decimal CalcularBono()
    {
        return 100;
    }
}

public class Vendedor : Empleado
{
    public decimal VentasMes { get; set; }

    public override decimal CalcularBono()
    {
        return + (VentasMes * 0.05m); //Bono base + comisión
    }
}

public class Gerente : Empleado
{
    public override decimal CalcularBono()
    {
        return 500;
    }
}*/

//====================================== 3. INTERFACES ====================================================
// El CONTRATO: cualquier forma de pago debe saber "procesar" un monto
/*List<IMetodoPago> pagos = new List<IMetodoPago>
{
    new PagoTarjeta(),
    new PagoEfectivo(),
    new PagoTransferencia()
};

foreach (IMetodoPago pago in pagos)
{
    // MISMO mensaje (Procesar), cada objeto responde a su manera
    Console.WriteLine(pago.Procesar(500));
}

public interface IMetodoPago
{
    string Procesar(decimal monto);
}

// Cada clase implementa el contrato a su propia manera
public class PagoTarjeta : IMetodoPago
{
    public string Procesar(decimal monto)
    {
        return $"Cobrando ${monto} a la tarjeta con un cargo de $2 por procesamiento.";
    }
}

public class PagoEfectivo : IMetodoPago
{
    public string Procesar(decimal monto)
    {
        return $"Recibiendo ${monto} en efectivo, sin cargos adicionales.";
    }
}

public class PagoTransferencia : IMetodoPago
{
    public string Procesar(decimal monto)
    {
        return $"Transferencia de ${monto} procesada, disponible en 24 horas.";
    }
}*/

//====================================== 4. CLASES ABSTRACTAS ====================================================
/*List<Figura> figuras = new List<Figura>
{
    new Circulo(5),
    new Rectangulo(4, 6)
};

foreach (Figura f in figuras)
{
    f.MostrarInfo(); // usa el método compartido, que internamente llama a CalcularArea()
}


public abstract class Figura
{
    // Campo/propiedad compartido por todas las figuras
    public string Nombre { get; set; }

    // Constructor: aunque no se pueda instanciar Figura directamente,
    // las clases hijas SÍ lo usan mediante base(...)
    public Figura(string nombre)
    {
        Nombre = nombre;
    }

    // Método ABSTRACTO: no tiene cuerpo, cada hija DEBE implementarlo
    public abstract double CalcularArea();

    // Método NORMAL (con cuerpo): se comparte automáticamente,
    // las hijas no necesitan reescribirlo
    public void MostrarInfo()
    {
        Console.WriteLine($"{Nombre} tiene un área de {CalcularArea():F2}");
    }
}

public class Circulo : Figura
{
    public double Radio { get; set; }

    public Circulo(double radio) : base("Círculo")
    {
        Radio = radio;
    }

    // Obligatorio: si no lo implementamos, no compila
    public override double CalcularArea()
    {
        return Math.PI * Radio * Radio;
    }
}

public class Rectangulo : Figura
{
    public double Base { get; set; }
    public double Altura { get; set; }

    public Rectangulo(double baseValor, double altura) : base("Rectángulo")
    {
        Base = baseValor;
        Altura = altura;
    }

    public override double CalcularArea()
    {
        return Base * Altura;
    }
}*/

//====================================== 5. COMPOSICION SOBRE HERENCIA ====================================================
/*Ave aguila = new Ave(new Volar());
Ave pinguino = new Ave(new Nadar());
Ave gallina = new Ave(new Caminar());

aguila.Moverse();    // Estoy volando
pinguino.Moverse();  // Estoy nadando
gallina.Moverse();   // Estoy caminando

//Enfoque con herencia (rígido)
// public class Ave
// {
//     public virtual void Volar()
//     {
//         Console.WriteLine("Estoy volando");
//     }
// }

// public class Pinguino : Ave
// {
//     //Problema: un pingüino es un ave, pero no puede volar
//     public override void Volar()
//     {
//         throw new NotSupportedException("Los pingüinos no vuelan");
//     }
// }

// Composición (comportamientos independientes)
// ---------- Piezas de comportamiento (independientes) ----------
public interface IMovimiento
{
    void Mover();
}

public class Volar : IMovimiento
{
    public void Mover() => Console.WriteLine("Estoy volando");
}

public class Nadar : IMovimiento
{
    public void Mover() => Console.WriteLine("Estoy nadando");
}

public class Caminar : IMovimiento
{
    public void Mover() => Console.WriteLine("Estoy caminando");
}

// ---------- Clase Ave: "tiene un" comportamiento de movimiento ----------
public class Ave
{
    private readonly IMovimiento _movimiento;

    // Composición: el comportamiento se recibe desde afuera (constructor)
    public Ave(IMovimiento movimiento)
    {
        _movimiento = movimiento;
    }

    public void Moverse()
    {
        _movimiento.Mover(); // delega el trabajo a la pieza compuesta
    }
}*/

//====================================== 6. PATRON REPOSITORY ====================================================
// Se instancia la clase concreta, pero se referencia con el tipo de la interfaz
/*IClienteRepository repositorio = new ClienteRepositoryMemoria();

repositorio.Agregar(new Cliente { Id = 1, Nombre = "Ana" });
repositorio.Agregar(new Cliente { Id = 2, Nombre = "Luis" });

Cliente encontrado = repositorio.ObtenerPorId(1);
Console.WriteLine(encontrado.Nombre); // Ana

List<Cliente> todos = repositorio.ObtenerTodos();
foreach (var c in todos)
{
    Console.WriteLine($"{c.Id} - {c.Nombre}");
}


//Modelo
public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; }
}

//Interfaz (Contrato)
public interface IClienteRepository
{
    Cliente ObtenerPorId(int id);
    void Agregar(Cliente cliente);
    List<Cliente> ObtenerTodos();
}

//Clase que implementa la interfaz
public class ClienteRepositoryMemoria : IClienteRepository
{
    private readonly List<Cliente> _clientes = new();

    public Cliente ObtenerPorId(int id)
    {
        return _clientes.FirstOrDefault(c => c.Id == id);
    }

    public void Agregar(Cliente cliente)
    {
        _clientes.Add(cliente);
    }

    public List<Cliente> ObtenerTodos()
    {
        return _clientes;
    }
}*/

//====================================== 7. DELEGADOS ====================================================
//Crear métodos normales que coincidan con esa forma
// int Sumar(int a, int b) => a + b;
// int Restar(int a, int b) => a - b;

// Operacion operacion = Sumar;
// Console.WriteLine(operacion(5, 3)); // 8

// operacion = Restar;
// Console.WriteLine(operacion(5, 3)); // 2

// public delegate int Operacion(int a, int b);

//====================================== 8. EXPRESIONES LAMBDA ====================================================
// Un solo parámetro
/*Func<int, int> alCuadrado = x => x * x;
Console.WriteLine(alCuadrado(4)); // 16

// Sin parámetros
Func<string> saludo = () => "Hola!";
Console.WriteLine(saludo()); // Hola!

// Sin retorno (Action)
Action<string> imprimir = texto => Console.WriteLine($">> {texto}");
imprimir("Probando lambda"); // >> Probando lambda

//En listas, forma mas usada
List<int> numeros = new List<int> { 2, 5, 8, 11, 14 };

Console.WriteLine("====================== MAYORES A 5 ==============================");

var mayoresA5 = numeros.Where(n => n > 5);
foreach(var num in mayoresA5)
    Console.WriteLine(num);

Console.WriteLine("====================== DUPLICADOS ==============================");

var duplicados = numeros.Select(n => n * 2);
foreach(var dup in duplicados)
    Console.WriteLine(dup);*/

//====================================== 9. EVENTOS ====================================================
/*Alarma alarma = new Alarma();
Sirena sirena = new Sirena();
NotificacionCelular notificacion = new NotificacionCelular();

// Suscripción: "cuando la alarma se active, ejecuta este método"
alarma.Activada += sirena.Sonar!;
alarma.Activada += notificacion.Enviar!;

alarma.Activar();

public class Alarma
{
    //Se declara el evento, basado en un delegado (EventHandler es un delegado predefinido por C#)
    public event EventHandler Activada;

    public void Activar()
    {
        Console.WriteLine("La alarma se está activando...");

        // 2. Se "dispara" el evento, avisando a todos los suscriptores
        Activada?.Invoke(this, EventArgs.Empty);
    }
}

public class Sirena
{
    public void Sonar(object sender, EventArgs e)
    {
        Console.WriteLine("Sirena sonando");
    }
}

public class NotificacionCelular
{
    public void Enviar(object sender, EventArgs e)
    {
        Console.WriteLine("Notificación enviada al celular");
    }
}*/