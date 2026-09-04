namespace ProgramacionOrientadoAObjetos;

public class Empleado
{
    public string Nombre { get; private set; }
    public decimal SalarioBase { get; private set; }
    public int AniosExperiencia { get; private set; }

    public Empleado(string nombre, decimal salarioBase, int aniosExperiencia)
    {
        Nombre = nombre;
        SalarioBase = salarioBase;
        AniosExperiencia = aniosExperiencia;
    }

    public virtual decimal CalcularBono()
    {
        return SalarioBase * 0.05m;
    }

    public override string ToString()
    {
        return $" Puesto: { GetType().Name } | Nombre: {Nombre} | Salario: {SalarioBase:C} | Bono: {CalcularBono():C}";
    }
}
